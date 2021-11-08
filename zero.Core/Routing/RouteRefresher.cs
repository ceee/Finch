using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Database.Indexes;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class RouteRefresher : Routes, IRouteRefresher
  {
    public RouteRefresher(IZeroContext context, IZeroStore store, ILogger<Routes> logger, IEnumerable<IRouteProvider> providers) : base(context, store, logger, providers)
    {

    }

    public class Res : RouteUpdaterResult
    {
      public List<Route> AffectedRoutes { get; set; }
    }

    public async Task<Res> Test<T>(bool isFake, T model, T previousModel = default) where T : IZeroRouteEntity
    {
      // find provider for this model
      if (!TryGetProvider(model, out IRouteProvider provider))
      {
        return new();
      }

      RoutingContext context = GetContext();

      // check if route old route is stale
      if (previousModel != null && !(await provider.IsRouteStale(context, previousModel, model)))
      {
        return new();
      }

      // build new route
      Route route = await provider.Create(context, model);

      if (route == null)
      {
        return new();
      }

      route.ProviderAlias = provider.Alias;

      string id = model.Id;
      List<Route> dependentRoutes = await context.Session.Query<Route, Routes_ByDependencies>().Where(x => x.Dependencies.ContainsAny(new[] { id })).ToListAsync();
      var routeGroups = dependentRoutes.GroupBy(x => x.ProviderAlias);

      RouteUpdaterContext updaterContext = new(context.Store, context.Context, context.Session);
      Res result = new();
      result.AffectedRoutes = dependentRoutes;

      foreach (var routeGroup in routeGroups)
      {
        if (!TryGetProvider(routeGroup.Key, out IRouteProvider dependentProvider))
        {
          continue;
        }

        updaterContext.AffectedRoutes = routeGroup;

        foreach (RouteUpdater updater in dependentProvider.Updaters)
        {
          if (updater.CanHandle(model))
          {
            RouteUpdaterResult updaterResult = await updater.OnUpdate(updaterContext, model, previousModel);

            result.Updated.AddRange(updaterResult.Updated);
            result.Removed.AddRange(updaterResult.Removed);

            if (!isFake)
            {
              //foreach (Route removedRoute in updaterResult.Removed)
              //{
              //  context.Session.Delete(removedRoute);
              //}

              foreach (Route updatedRoute in updaterResult.Updated)
              {
                await context.Session.StoreAsync(updatedRoute);
              }
            }
          }
        }
      }

      if (!isFake)
      {
        await context.Session.SaveChangesAsync();
      }

      return result;
    }


    public async Task<RouteUpdaterResult> RunProviderUpdaters<T>(RouteUpdaterContext context, IRouteProvider provider, T model, T previousModel = default) where T : IZeroRouteEntity
    {
      RouteUpdaterResult result = new();

      foreach (RouteUpdater updater in provider.Updaters)
      {
        if (updater.CanHandle(model))
        {
          RouteUpdaterResult updaterResult = await updater.OnUpdate(context, model, previousModel);
          result.Removed.AddRange(updaterResult.Removed);
          result.Updated.AddRange(updaterResult.Updated);
        }
      }

      return result;
    }


    public async Task<bool> UpdateOnDemand<T>(T model, T previousModel = default) where T : IZeroRouteEntity 
    {
      // find provider for this model
      if (!TryGetProvider(model, out IRouteProvider provider))
      {
        return false;
      }

      RoutingContext context = GetContext();

      // check if route old route is stale
      if (previousModel != null && !(await provider.IsRouteStale(context, previousModel, model)))
      {
        return false;
      }

      // build new route
      Route route = await provider.Create(context, model);

      if (route == null)
      {
        return false;
      }

      route.ProviderAlias = provider.Alias;

      // delete previous route if the ID has changed,
      // otherwise it will just be updated
      if (previousModel != null && !route.Id.Equals(provider.Id(previousModel)))
      {
        context.Session.Delete(previousModel);
      }

      await context.Session.StoreAsync(route);
      await UpdateDependencies(context, model);
      await context.Session.SaveChangesAsync();

      return true;
    }


    protected async Task UpdateDependencies<T>(RoutingContext context, T model) where T : IZeroRouteEntity
    {
      List<Route> dependentRoutes = await context.Session.Query<Route, Routes_ByDependencies>().Where(x => model.Id.In(x.Dependencies)).ToListAsync();
    }
  }


  public interface IRouteRefresher
  {

  }
}
