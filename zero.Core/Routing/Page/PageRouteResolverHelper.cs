using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using zero.Core.Database.Indexes;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class PageRouteResolverHelper
  {
    protected ConcurrentDictionary<string, HashSet<(Page, Route)>> PageRouteCache { get; set; } = new();


    public async Task<Route> ResolveFor<T>(RoutingContext context, Func<Page, bool> predicate = null)
    {
      return (await ResolveAllFor<T>(context, predicate)).FirstOrDefault();
    }


    public async Task<Page> ResolvePageFor<T>(RoutingContext context, Func<Page, bool> predicate = null)
    {
      return (await ResolveAllPagesFor<T>(context, predicate)).FirstOrDefault();
    }


    public async Task<HashSet<Route>> ResolveAllFor<T>(RoutingContext context, Func<Page, bool> predicate = null)
    {
      string fullKey = typeof(T).Name.ToString().ToLower() + ":" + context.Context.AppId;

      if (!PageRouteCache.TryGetValue(fullKey, out HashSet<(Page, Route)> routes))
      {
        List<Page> pages = new();
        IEnumerable<Expression<Func<Page, bool>>> resolvers = context.Context.Options.Routing.PageResolvers.GetAll(typeof(T));

        foreach (Expression<Func<Page, bool>> resolver in resolvers.Reverse())
        {
          pages.AddRange(await context.Session.Query<Page>().Where(resolver).ToListAsync());
        }

        Dictionary<string, Page> idToPage = pages.ToDictionary(x => context.Context.Options.Routing.PageRouteIdBuilder.Generate(x), x => x);
        Dictionary<string, Route> idToRoute = await context.Session.LoadAsync<Route>(idToPage.Select(x => x.Key));

        routes = new();

        foreach ((string id, Page page) in idToPage)
        {
          if (idToRoute.TryGetValue(id, out Route route) && route != null)
          {
            routes.Add((page, route));
          }
        }

        PageRouteCache.TryAdd(fullKey, routes);
      }

      if (predicate != null)
      {
        return routes.Where(x => predicate(x.Item1)).Select(x => x.Item2).ToHashSet();
      }

      return routes.Select(x => x.Item2).ToHashSet();
    }


    public async Task<HashSet<Page>> ResolveAllPagesFor<T>(RoutingContext context, Func<Page, bool> predicate = null)
    {
      string fullKey = typeof(T).Name.ToString().ToLower() + ":" + context.Context.AppId;

      if (!PageRouteCache.TryGetValue(fullKey, out HashSet<(Page, Route)> routes))
      {
        List<Page> pages = new();
        IEnumerable<Expression<Func<Page, bool>>> resolvers = context.Context.Options.Routing.PageResolvers.GetAll(typeof(T));

        foreach (Expression<Func<Page, bool>> resolver in resolvers.Reverse())
        {
          pages.AddRange(await context.Session.Query<Page>().Where(resolver).ToListAsync());
        }

        Dictionary<string, Page> idToPage = pages.ToDictionary(x => context.Context.Options.Routing.PageRouteIdBuilder.Generate(x), x => x);
        Dictionary<string, Route> idToRoute = await context.Session.LoadAsync<Route>(idToPage.Select(x => x.Key));

        routes = new();

        foreach ((string id, Page page) in idToPage)
        {
          if (idToRoute.TryGetValue(id, out Route route) && route != null)
          {
            routes.Add((page, route));
          }
        }

        PageRouteCache.TryAdd(fullKey, routes);
      }

      if (predicate != null)
      {
        return routes.Where(x => predicate(x.Item1)).Select(x => x.Item1).ToHashSet();
      }

      return routes.Select(x => x.Item1).ToHashSet();
    }


    public async Task<Dictionary<Page, Route>> ResolveAllForAsDictionary<T>(RoutingContext context, Func<(Page, Route), bool> predicate = null)
    {
      string fullKey = typeof(T).Name.ToString().ToLower() + ":" + context.Context.AppId;

      if (!PageRouteCache.TryGetValue(fullKey, out HashSet<(Page, Route)> routes))
      {
        List<Page> pages = new();
        IEnumerable<Expression<Func<Page, bool>>> resolvers = context.Context.Options.Routing.PageResolvers.GetAll(typeof(T));

        foreach (Expression<Func<Page, bool>> resolver in resolvers.Reverse())
        {
          pages.AddRange(await context.Session.Query<Page>().Where(resolver).ToListAsync());
        }

        Dictionary<string, Page> idToPage = pages.ToDictionary(x => context.Context.Options.Routing.PageRouteIdBuilder.Generate(x), x => x);
        Dictionary<string, Route> idToRoute = await context.Session.LoadAsync<Route>(idToPage.Select(x => x.Key));

        routes = new();

        foreach ((string id, Page page) in idToPage)
        {
          if (idToRoute.TryGetValue(id, out Route route) && route != null)
          {
            routes.Add((page, route));
          }
        }

        PageRouteCache.TryAdd(fullKey, routes);
      }

      if (predicate != null)
      {
        return routes.Where(x => predicate(x)).ToDictionary(x => x.Item1, x => x.Item2);
      }

      return routes.ToDictionary(x => x.Item1, x => x.Item2);
    }


    public async Task<bool> IsRelevantFor<T>(RoutingContext context, Page page)
    {
      HashSet<Page> pages = await ResolveAllPagesFor<T>(context);

      if (pages.Any(x => x.Id == page.Id))
      {
        return true;
      }

      IList<Page> children = await GetChildren(context, pages.Select(x => x.Id));
      return children.Any(x => x.Id == page.Id);
    }


    /// <summary>
    /// Get relevant pages for a page
    /// </summary>
    public virtual async Task<List<Page>> GetRelevantPagesFor<T>(RoutingContext context, Page page)
    {
      HashSet<Page> pages = await ResolveAllPagesFor<T>(context);

      if (pages.Any(x => x.Id == page.Id))
      {
        return new();
      }

      return await GetChildren(context, pages.Select(x => x.Id));
    }


    /// <summary>
    /// Get parents for a page
    /// </summary>
    protected virtual async Task<List<Page>> GetChildren(RoutingContext context, IEnumerable<string> pageIds)
    {
      return await context.Session.Query<Pages_ByHierarchy.Result, Pages_ByHierarchy>()
        .Where(x => x.PathIds.In(pageIds))
        .ProjectInto<Page>()
        .ToListAsync();
    }
  }
}
