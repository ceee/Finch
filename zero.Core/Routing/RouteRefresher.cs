using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;
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
      if (previousModel != null && !route.Id.Equals(GetId(provider, previousModel)))
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
