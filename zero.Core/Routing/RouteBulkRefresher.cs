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
    protected IZeroContext Context { get; set; }
    protected IEnumerable<IRouteProvider> Providers { get; set; }


    public RouteBulkRefresher(IZeroStore store, IZeroContext context, IEnumerable<IRouteProvider> providers)
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
        using ZeroContextScope scope = Context.CreateScope(app);
        using IZeroDocumentSession session = scope.Store.Session(scope.Database, ZeroSessionResolution.Create);
        session.Advanced.MaxNumberOfRequestsPerSession = 100_000;

        await scope.Store.Raven.PurgeAsync<Route>(scope.Database);

        RoutingContext context = new(scope.Store, scope.Context, session);

        foreach (IRouteProvider provider in Providers)
        {
          using BulkInsertOperation bulkInsert = scope.Store.Raven.BulkInsert(scope.Database);

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
