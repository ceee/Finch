using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Database;
using zero.Core.Database.Indexes;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class ZeroEntityRouteInterceptor : CollectionInterceptor
  {
    protected IZeroContext Context { get; set; }

    protected IZeroStore Store { get; set; }

    protected IEnumerable<IRouteProvider> Providers { get; set; }

    protected IRoutes Routes { get; set; }

    protected ILogger<ZeroEntityRouteInterceptor> Logger { get; set; }


    public ZeroEntityRouteInterceptor(IZeroContext context, IZeroStore store, IRoutes routes, ILogger<ZeroEntityRouteInterceptor> logger, IEnumerable<IRouteProvider> providers)
    {
      Context = context;
      Store = store;
      Routes = routes;
      Providers = providers;
      Logger = logger;
    }


    /// <inheritdoc />
    public override async Task Saved(SaveParameters args)
    {
      // find provider for this model
      if (!Routes.TryGetProvider(args.Model, out IRouteProvider provider))
      {
        return;
      }

      List<Route> updatedRoutes = new();
      RoutingContext context = GetContext();

      // return if the route is not stale
      ZeroEntity previousModel = null;
      if (previousModel != null && !(await provider.IsRouteStale(context, previousModel, args.Model)))
      {
        return;
      }

      // build and save new route
      Route route = await provider.Create(context, args.Model);
      updatedRoutes.Add(route);
      await context.Session.StoreAsync(route);

      // delete obsolete routes
      if (previousModel != null)
      {
        await RemoveDependencies(args.Id);
      }

      // find all providers which need to be informed 
      // that this route has changed

      // e.g. a Page has changed:
      // 1. Inform PageRouteProvider to update children
      // 2. Inform ProductRouteProvider to update product routes which are part of this page or of a child page
      // 3. ...

      // in a dependent route provider:
      // 1. subscribe to <Page> updates
      // 2. determine if this Page is relevant for the provider
      // 3. gather all updated routes from the provider

      foreach (IRouteProvider dependentProvider in Providers.OrderByDescending(x => x.Priority))
      {
        await foreach (Route dependentRoute in dependentProvider.SeedOnUpdate(context, args.Model))
        {
          updatedRoutes.Add(dependentRoute);
          await context.Session.StoreAsync(dependentRoute);
        }
      }

      await context.Session.SaveChangesAsync();
    }


    /// <inheritdoc />
    public override async Task Deleted(DeleteParameters args)
    {
      await RemoveDependencies(args.Id);
    }


    /// <summary>
    /// Delete all routes which have the affected entity as a dependency
    /// </summary>
    protected virtual async Task RemoveDependencies(string id)
    {
      await Store.Raven.PurgeAsync<Route>(Context.Application.Database, $"where c.Dependencies IN ($id)", new Raven.Client.Parameters()
      {
        { "id", id }
      });
    }


    protected async Task UpdateDependencies<T>(RoutingContext context, T model) where T : IZeroRouteEntity
    {
      List<Route> dependentRoutes = await context.Session.Query<Route, Routes_ByDependencies>().Where(x => model.Id.In(x.Dependencies)).ToListAsync();
    }


    /// <summary>
    /// Build a new routing context
    /// </summary>
    protected virtual RoutingContext GetContext()
    {
      return new(Store, Context, Store.Session());
    }
  }
}
