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
  public class ZeroEntityRouteInterceptor : CollectionInterceptor<ZeroEntity>
  {
    protected IZeroContext Context { get; set; }

    protected IZeroStore Store { get; set; }

    protected IEnumerable<IRouteProvider> Providers { get; set; }

    protected IRoutes Routes { get; set; }

    protected IRedirectAutomation RedirectAutomation { get; set; }

    protected ILogger<ZeroEntityRouteInterceptor> Logger { get; set; }


    public ZeroEntityRouteInterceptor(IZeroContext context, IZeroStore store, IRoutes routes, ILogger<ZeroEntityRouteInterceptor> logger, IEnumerable<IRouteProvider> providers, IRedirectAutomation redirectAutomation)
    {
      Context = context;
      Store = store;
      Routes = routes;
      Providers = providers;
      Logger = logger;
      RedirectAutomation = redirectAutomation;
    }


    /// <inheritdoc />
    public override async Task Saved(InterceptorParameters args, ZeroEntity model)
    {
      // DONE [1] assume we have an update for a Product:
      // this will not trigger a new seeding as the ProductRouteProvider handles <ProductRouteParams> entities, but not products itself
      // we could overwrite CanHandle() to support <Product> types, but this will lead to errors when other methods are called on this provider

      // DONE [2] some providers have dependencies on entities which are not part of a standalone route provider.
      // Example: The ProductRouteProvider has a dependency on <Channel>. Channel has no route provider for itself.
      // Therefore updates to Channels will not trigger seeding.
      // For deletes it's easy as we can just delete all routes with the dependency by ID (and we won't need a corresponding route provider)

      // DONE [3] we have to find all routes which have become obsolete after update seeding.

      // TODO [4] if the entity is part of a route provider it can be checked for stale-ness.
      // The problem is if we skip this part and only update dependencies.
      // E.g. ProductRouteProvider has a dependency on <Channel>. As Channel has no route provider it can not be checked for stale-ness.
      // This has to be done in ProductRouteProvider as only the provider knows which properties of the entity it is using (therefore which properties need to be compared).

      RoutingContext context = GetContext();
      context.Session.Advanced.MaxNumberOfRequestsPerSession = 100_000;

      Dictionary<string, string> urlUpdates = new();
      List<Route> obsoleteRoutes = await GetDependencies(context, model);
      int countObsoleteRoutes = obsoleteRoutes.Count;
      int countRoutes = 0;


      // routine to store a new or updated route
      async Task StoreRoute(Route route)
      {
        if (route != null)
        {
          countRoutes += 1;

          Route obsoleteRoute = obsoleteRoutes.FirstOrDefault(x => x.Id == route.Id);

          if (obsoleteRoute != null)
          {
            obsoleteRoutes.Remove(obsoleteRoute);
            urlUpdates.Add(obsoleteRoute.Url, route.Url);
            route = obsoleteRoute.Update(route);
          }

          await context.Session.StoreAsync(route);
        }
      }


      // find provider for this model
      if (Routes.TryGetProvider(model, out IRouteProvider provider))
      {
        // return if the route is not stale
        ZeroEntity previousModel = null;
        if (previousModel != null && !(await provider.IsRouteStale(context, previousModel, model)))
        {
          return;
        }

        // build and save new route
        Route route = await provider.Create(context, model);
        await StoreRoute(route);
      }

      // find all providers which need to be informed that this route has changed
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
        await foreach (Route dependentRoute in dependentProvider.SeedOnUpdate(context, model))
        {
          await StoreRoute(dependentRoute);
        }
      }

      // at first we have gathered all routes which are dependent on the entity (via GetDependencies())
      // every time we insert a new route in the route table we remove this route from the gathered routes above.
      // what is left are routes which are obsolete and have not been updated
      foreach (Route obsoleteRoute in obsoleteRoutes)
      {
        context.Session.Delete(obsoleteRoute);
      }

      await context.Session.SaveChangesAsync();

      // delete associated redirects for obsolete routes
      await RedirectAutomation.DeleteForRoutes(obsoleteRoutes.ToArray());

      // update associated redirects for routes which have a new URL
      foreach (var kvp in urlUpdates)
      {
        await RedirectAutomation.AddForRoute(kvp.Key, kvp.Value);
      }

      int countUpdatedRoutes = countObsoleteRoutes - obsoleteRoutes.Count;
      Logger.LogInformation("Route updates completed (+{added}/~{updated}/-{removed}) for {model} (id: {id})", countRoutes - countUpdatedRoutes, countUpdatedRoutes, obsoleteRoutes.Count, model.Name, model.Id);
    }


    /// <inheritdoc />
    public override async Task Deleted(InterceptorParameters args, ZeroEntity model)
    {
      RoutingContext context = GetContext();

      string id = model.Id;
      List<Route> dependencies = await GetDependencies(context, model);

      await context.Store.Raven.PurgeAsync<Route>(context.Context.Application.Database, $"where c.Dependencies IN ($id)", new Raven.Client.Parameters()
      {
        { "id", id }
      });

      // delete associated redirects for obsolete routes
      await RedirectAutomation.DeleteForRoutes(dependencies.ToArray());

      Logger.LogInformation("Route deletes completed (-{removed}) for {model} (id: {id})", dependencies.Count, model.Name, model.Id);
    }


    /// <summary>
    /// Get route dependencies for an entity
    /// </summary>
    protected async Task<List<Route>> GetDependencies<T>(RoutingContext context, T model) where T : IZeroRouteEntity
    {
      string[] ids = new[] { model.Id };
      return await context.Session.Query<Route, Routes_ByDependencies>().Where(x => x.Dependencies.ContainsAny(ids)).ToListAsync();
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
