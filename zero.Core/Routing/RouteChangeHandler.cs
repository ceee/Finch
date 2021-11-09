using Microsoft.Extensions.Logging;
using Raven.Client;
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
  public class RouteChangeHandler : Routes, IRouteChangeHandler
  {
    public RouteChangeHandler(IZeroContext context, IZeroStore store, ILogger<Routes> logger, IEnumerable<IRouteProvider> providers) : base(context, store, logger, providers)
    {

    }


    public async Task OnEntityDelete<T>(T model) where T : IZeroRouteEntity
    {
      RoutingContext context = GetContext();
      await RemoveDependencies(context, model);
    }


    /// <summary>
    /// Removes dependent routes of an entity
    /// </summary>
    protected virtual async Task RemoveDependencies<T>(RoutingContext context, T model) where T : IZeroRouteEntity
    {
      await context.Store.Raven.PurgeAsync<Route>(context.Context.Application.Database, $"where c.Dependencies IN ($id)", new Parameters()
      {
        { "id", model.Id }
      });
    }


    //protected async Task<List<Route>> GetDependencies<T>(RoutingContext context, T model) where T : IZeroRouteEntity
    //{
    //  return await context.Session.Query<Route, Routes_ByDependencies>().Where(x => model.Id.In(x.Dependencies)).ToListAsync();
    //}
  }


  public interface IRouteChangeHandler
  {

  }
}
