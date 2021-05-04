using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Options;

namespace zero.Core.Routing
{
  public class PageRouteProvider : AbtractRouteProvider<Page>
  {
    protected const string REF_KEY = "page";
    protected const string PAGE_TYPE_KEY = "pageType";

    protected ILogger<PageRouteProvider> Logger { get; set; }

    protected IPageUrlBuilder UrlBuilder { get; set; }


    public PageRouteProvider(IZeroOptions options, ILogger<PageRouteProvider> logger, IPageUrlBuilder urlBuilder) : base("zero.pages", options)
    {
      Logger = logger;
      UrlBuilder = urlBuilder;
    }


    /// <inheritdoc />
    public override string GetRouteId(Page model, object parameters = null) => ID_PREFIX + model.Hash;


    /// <inheritdoc />
    public override async Task<IResolvedRoute> ResolveRoute(IAsyncDocumentSession session, Route route)
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

      Dictionary<string, Page> pages = await session.LoadAsync<Page>(ids);

      if (!pages.TryGetValue(reference.Id, out Page page))
      {
        return null;
      }

      resolved.Page = page;
      resolved.Parents = pages.Where(x => x.Key != reference.Id).Select(x => x.Value).ToList();

      return resolved;
    }


    /// <inheritdoc />
    public override async Task<IList<Route>> GetAllRoutes(IAsyncDocumentSession session)
    {
      IList<Route> TraversePageChildren(Page parent, IEnumerable<Page> parents, IEnumerable<Page> allPages)
      {
        List<Route> routes = new List<Route>();
        IEnumerable<Page> currentPages = allPages.Where(x => x.ParentId == parent?.Id);

        foreach (Page page in currentPages)
        {
          Route route = BuildRoute(page, parents, allPages);

          if (route != null)
          {
            routes.Add(route);
          }

          routes.AddRange(TraversePageChildren(page, parents.Union(new List<Page>() { page }), allPages));
        }

        return routes;
      }

      IList<Page> pages = await session.Query<Page>().ToListAsync();
      return TraversePageChildren(null, new List<Page>() { }, pages);
    }


    /// <summary>
    /// Build route entity from page
    /// </summary>
    protected virtual Route BuildRoute(Page page, IEnumerable<Page> parents, IEnumerable<Page> allPages)
    {
      if (page is PageFolder)
      {
        return null;
      }

      Route route = new Route()
      {
        Id = GetRouteId(page),
        Url = UrlBuilder.GetUrl(page, parents),
        ProviderAlias = Alias
      };

      route.Params.Add(PAGE_TYPE_KEY, page.PageTypeAlias);
      route.References.Add(new RouteReference(page.Id, REF_KEY));

      if (parents != null)
      {
        foreach (Page parent in parents)
        {
          route.Dependencies.Add(parent.Id);
        }
      }

      return route;
    }
  }
}
