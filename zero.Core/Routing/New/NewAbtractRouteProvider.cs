using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Options;

namespace zero.Core.Routing
{
  public abstract class NewAbtractRouteProvider<T> : INewRouteProvider<T> where T : IZeroRouteEntity
  {
    public virtual string Alias { get; protected set; }

    public const string ID_PREFIX = "routes.";

    public const char SLASH = '/';

    protected IZeroStore Store { get; set; }

    protected IZeroOptions Options { get; set; }



    public NewAbtractRouteProvider(string alias, IZeroStore store, IZeroOptions options)
    {
      Store = store;
      Alias = alias;
      Options = options;
    }


    public virtual Task Warmup() => Task.CompletedTask;


    public virtual string Id(T model) => ID_PREFIX + model.Hash;


    public abstract string Url(T model);


    public abstract Route Create(T model);


    public abstract Task<IEnumerable<T>> All();


    public abstract Task<IResolvedRoute> Resolve(RouteResponse response);







    protected async Task<Page> ResolvePage()
    {
      IZeroDocumentSession session = Store.Session();
      IEnumerable<Expression<Func<Page, bool>>> resolvers = Options.Routing.PageResolvers.GetAll(typeof(T));

      foreach (Expression<Func<Page, bool>> resolver in resolvers.Reverse())
      {
        Page page = await session.Query<Page>().FirstOrDefaultAsync(resolver);

        if (page != null)
        {
          return page;
        }
      }

      return null;
    }


    protected async Task<IEnumerable<Page>> ResolvePages()
    {
      IZeroDocumentSession session = Store.Session();
      List<Page> pages = new();
      IEnumerable<Expression<Func<Page, bool>>> resolvers = Options.Routing.PageResolvers.GetAll(typeof(T));

      foreach (Expression<Func<Page, bool>> resolver in resolvers.Reverse())
      {
        pages.AddRange(await session.Query<Page>().Where(resolver).ToListAsync());
      }

      return pages;
    }


    protected async Task<Route> ResolvePageRoute()
    {
      Page page = await ResolvePage();

      // WARNING: we are assuming that the route id is built from the page hash but this could be altered with PageRouteProvier.GetRouteId.
      // we cannot use a dependency on this provider here as we are working from the abstract route provider which is the base of the PageRouteProvider itself,
      // and therefore a circular dependency.
      return page == null ? null : await Store.Session().LoadAsync<Route>(ID_PREFIX + page.Hash);
    }


    protected async Task<IEnumerable<Route>> ResolvePageRoutes()
    {
      List<Route> routes = new();
      IEnumerable<Page> pages = await ResolvePages();

      string[] ids = pages.Select(x => ID_PREFIX + x.Hash).ToArray();
      return ids.Length < 1 ? routes : (await Store.Session().LoadAsync<Route>(ids)).Select(x => x.Value);
    }
  }
}
