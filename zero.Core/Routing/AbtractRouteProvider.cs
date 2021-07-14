using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Options;

namespace zero.Core.Routing
{
  public abstract class AbtractRouteProvider<T> : IRouteProvider<T>
  {
    public virtual string Alias { get; protected set; }

    public virtual Type[] AffectedTypes { get; protected set; }

    public const string ID_PREFIX = "routes.";

    public const char SLASH = '/';

    protected IZeroOptions Options { get; set; }



    public AbtractRouteProvider(string alias, IZeroOptions options)
    {
      Alias = alias;
      Options = options;
      AffectedTypes = new Type[1] { typeof(T) };
    }


    /// <summary>
    /// Map a route to an MVC endpoint
    /// </summary>
    public virtual RouteProviderEndpoint MapEndpoint(IResolvedRoute route)
    {
      IEnumerable<Func<IResolvedRoute, RouteProviderEndpoint>> resolvers = Options.Routing.EndpointResolvers.GetAll(route.GetType());

      foreach (Func<IResolvedRoute, RouteProviderEndpoint> resolver in resolvers.Reverse())
      {
        RouteProviderEndpoint endpoint = resolver(route);

        if (endpoint != null)
        {
          return endpoint;
        }
      }

      return Options.Routing.DefaultEndpoint;
    }


    /// <inheritdoc />
    public virtual async Task<Dictionary<T, Route>> GetRoutes(IAsyncDocumentSession session, IEnumerable<T> models, object parameters = null)
    {
      Dictionary<T, Route> result = new();
      Dictionary<string, T> routeMap = new();
      HashSet<string> routeIds = new();

      foreach (T model in models)
      {
        if (model == null)
        {
          continue;
        }
        string routeId = GetRouteId(model, parameters);
        routeIds.Add(routeId);
        routeMap.TryAdd(routeId, model);
      }

      Dictionary<string, Route> routes = await session.LoadAsync<Route>(routeIds);

      foreach ((string key, Route route) in routes)
      {
        if (routeMap.TryGetValue(key, out T model))
        {
          result.TryAdd(model, route);
        }
      }

      return result;
    }


    /// <inheritdoc />
    public virtual async Task<Dictionary<object, Route>> GetRoutes(IAsyncDocumentSession session, IEnumerable<object> models, object parameters = null)
    {
      return (await GetRoutes(session, models.Select(x => (T)x).ToArray(), parameters)).ToDictionary(x => (object)x.Key, x => x.Value);
    }


    /// <inheritdoc />
    public virtual async Task<Route> GetRoute(IAsyncDocumentSession session, T model, object parameters = null)
    {
      return await session.LoadAsync<Route>(GetRouteId(model, parameters));
    }


    /// <inheritdoc />
    public virtual async Task<Route> GetRoute(IAsyncDocumentSession session, object model, object parameters = null)
    {
      if (!(model is T))
      {
        return null;
      }
      return await GetRoute(session, (T)model, parameters);
    }


    /// <inheritdoc />
    public virtual Task<IResolvedRoute> ResolveRoute(IAsyncDocumentSession session, Route route)
    {
      DefaultResolvedRoute resolved = new DefaultResolvedRoute() { Route = route };
      return Task.FromResult((IResolvedRoute)resolved);
    }


    /// <inheritdoc />
    public abstract Task<IList<Route>> GetAllRoutes(IAsyncDocumentSession session);


    /// <inheritdoc />
    public abstract string GetRouteId(T model, object parameters = null);


    /// <inheritdoc />
    public string GetRouteId(object model, object parameters = null)
    {
      if (!(model is T))
      {
        throw new ArgumentException($"Parameter has to be of type {typeof(T)}", nameof(model));
      }

      return GetRouteId((T)model, parameters);
    }


    protected async Task<Page> ResolvePage(IAsyncDocumentSession session)
    {
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


    protected async Task<IEnumerable<Page>> ResolvePages(IAsyncDocumentSession session)
    {
      List<Page> pages = new();
      IEnumerable<Expression<Func<Page, bool>>> resolvers = Options.Routing.PageResolvers.GetAll(typeof(T));

      foreach (Expression<Func<Page, bool>> resolver in resolvers.Reverse())
      {
        pages.AddRange(await session.Query<Page>().Where(resolver).ToListAsync());
      }

      return pages;
    }


    protected async Task<Route> ResolvePageRoute(IAsyncDocumentSession session)
    {
      Page page = await ResolvePage(session);

      // WARNING: we are assuming that the route id is built from the page hash but this could be altered with PageRouteProvier.GetRouteId.
      // we cannot use a dependency on this provider here as we are working from the abstract route provider which is the base of the PageRouteProvider itself,
      // and therefore a circular dependency.
      return page == null ? null : await session.LoadAsync<Route>(ID_PREFIX + page.Hash);
    }


    protected async Task<IEnumerable<Route>> ResolvePageRoutes(IAsyncDocumentSession session)
    {
      List<Route> routes = new();
      IEnumerable<Page> pages = await ResolvePages(session);

      string[] ids = pages.Select(x => ID_PREFIX + x.Hash).ToArray();
      return ids.Length < 1 ? routes : (await session.LoadAsync<Route>(ids)).Select(x => x.Value);
    }
  }
}
