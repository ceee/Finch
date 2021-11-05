using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class NewRouteBulkRefresher
  {
    protected IZeroStore Store { get; set; }
    protected IZeroContext Context { get; set; }
    protected IEnumerable<INewRouteProvider> Providers { get; set; }


    public NewRouteBulkRefresher(IZeroStore store, IZeroContext context, IEnumerable<INewRouteProvider> providers)
    {
      Store = store;
      Context = context;
      Providers = providers;
    }


    /// <inheritdoc />
    public async Task<int> RebuildAllRoutes()
    {
      int count = 0;

      List<Application> apps = await Store.Session(global: true).Query<Application>().ToListAsync();

      foreach (Application app in apps)
      {
        IZeroDocumentSession session = Store.Session(app.Database, ZeroSessionResolution.Create);
        session.Advanced.MaxNumberOfRequestsPerSession = 100_000;

        await Store.Raven.PurgeAsync<Route>(app.Database);

        RoutingContext context = new(Store, Context, session);

        foreach (INewRouteProvider provider in Providers)
        {
          using BulkInsertOperation bulkInsert = Store.Raven.BulkInsert(app.Database);

          await foreach (Route route in provider.Seed(context))
          {
            if (route != null)
            {
              route.ProviderAlias = provider.Alias;
              await bulkInsert.StoreAsync(route, route.Id);
              count += 1;
            }
          }
        }
      }

      return count;
    }
  }
}
