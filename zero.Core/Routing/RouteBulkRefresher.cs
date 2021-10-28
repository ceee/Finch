using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class RouteBulkRefresher
  {
    protected IZeroStore Store { get; set; }
    protected IEnumerable<IRouteProvider> Providers { get; set; }


    public RouteBulkRefresher(IZeroStore store, IEnumerable<IRouteProvider> providers)
    {
      Store = store;
      Providers = providers;
    }


    /// <inheritdoc />
    public async Task RebuildAllRoutes()
    {
      int count = 0;

      List<Application> apps = await Store.Session(global: true).Query<Application>().ToListAsync();

      foreach (Application app in apps)
      {
        IAsyncDocumentSession session = Store.Session(app.Database, ZeroSessionResolution.Create);
        session.Advanced.MaxNumberOfRequestsPerSession = 1000;

        // delete all routes
        //await Store.PurgeAsync<Route>(app.Database);

        foreach (IRouteProvider provider in Providers)
        {
          // get all routes for this provider
          IList<Route> routes = await provider.GetAllRoutes(session);

          // delete all registered routes in the database for this provider
          //await Store.PurgeAsync<Route>(app.Database, $"where {nameof(Route.ProviderAlias)} = $alias", new Raven.Client.Parameters()
          //{
          //  { "alias", provider.Alias }
          //});

          // store new routes
          using (BulkInsertOperation bulkInsert = Store.BulkInsert(app.Database))
          {
            foreach (Route route in routes)
            {
              await bulkInsert.StoreAsync(route, route.Id);
              count += 1;
            }
          }
        }
      }
    }
  }
}
