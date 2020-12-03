using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class PageRouteProvider : AbtractRouteProvider<IPage>
  {
    protected const string REF_KEY = "page";
    protected const string PAGE_TYPE_KEY = "pageType";

    protected ILogger<PageRouteProvider> Logger { get; set; }

    protected IPageUrlBuilder UrlBuilder { get; set; }

    protected IPageEndpointResolver EndpointResolver { get; set; }


    public PageRouteProvider(ILogger<PageRouteProvider> logger, IPageUrlBuilder urlBuilder, IPageEndpointResolver endpointResolver) : base("zero.pages")
    {
      Logger = logger;
      UrlBuilder = urlBuilder;
      EndpointResolver = endpointResolver;
    }


    /// <inheritdoc />
    public override RouteProviderEndpoint MapEndpoint(IResolvedRoute route)
    {
      if (!(route is PageRoute))
      {
        return base.MapEndpoint(route);
      }
      return EndpointResolver.GetEndpoint(route as PageRoute);
    }


    /// <inheritdoc />
    public override string GetRouteId(IPage model) => ID_PREFIX + model.Hash;


    /// <inheritdoc />
    public override async Task<IResolvedRoute> ResolveRoute(IAsyncDocumentSession session, IRoute route)
    {
      PageRoute resolved = new PageRoute(route);

      List<string> ids = new List<string>();
      RouteReference reference = route.References.SingleOrDefault(x => x.Collection == REF_KEY);

      if (reference == null)
      {
        return null;
      }

      ids.Add(reference.Id);
      ids.AddRange(route.Dependencies);

      Dictionary<string, IPage> pages = await session.LoadAsync<IPage>(ids);

      if (!pages.TryGetValue(reference.Id, out IPage page))
      {
        return null;
      }

      resolved.Page = page;
      resolved.Parents = pages.Where(x => x.Key != reference.Id).Select(x => x.Value).ToList();

      return resolved;
    }


    /// <inheritdoc />
    public override async Task<IList<IRoute>> GetAllRoutes(IAsyncDocumentSession session)
    {
      IList<IRoute> TraversePageChildren(IPage parent, IEnumerable<IPage> parents, IEnumerable<IPage> allPages)
      {
        List<IRoute> routes = new List<IRoute>();
        IEnumerable<IPage> currentPages = allPages.Where(x => x.ParentId == parent?.Id);

        foreach (IPage page in currentPages)
        {
          IRoute route = BuildRoute(page, parents, allPages);

          if (route != null)
          {
            routes.Add(route);
          }

          routes.AddRange(TraversePageChildren(page, parents.Union(new List<IPage>() { page }), allPages));
        }

        return routes;
      }

      IList<IPage> pages = await session.Query<IPage>().ToListAsync();
      return TraversePageChildren(null, new List<IPage>() { }, pages);
    }


    /// <summary>
    /// Build route entity from page
    /// </summary>
    protected virtual IRoute BuildRoute(IPage page, IEnumerable<IPage> parents, IEnumerable<IPage> allPages)
    {
      if (page is PageFolder)
      {
        return null;
      }

      IRoute route = new Route()
      {
        Id = GetRouteId(page),
        Url = UrlBuilder.GetUrl(page, parents),
        ProviderAlias = Alias
      };

      route.Params.Add(PAGE_TYPE_KEY, page.PageTypeAlias);
      route.References.Add(new RouteReference(page.Id, REF_KEY));

      if (parents != null)
      {
        foreach (IPage parent in parents)
        {
          route.Dependencies.Add(parent.Id);
        }
      }

      return route;
    }
  }
}
