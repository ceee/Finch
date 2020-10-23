using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Messages;
using zero.Core.Utils;

namespace zero.Core.Routing
{
  public class PageUrlProvider : IUrlProvider<PageRoute>
  {
    public string Alias => "zero.pages";

    public const string PAGE_COLLECTION = "page";

    public const string PAGE_TYPE = "pageType";

    protected IDocumentStore Raven { get; private set; }

    protected IMessageAggregator Messages { get; private set; }

    protected IPageUrlResolver PageUrlResolver { get; set; }


    protected IApplicationContext Context { get; private set; }

    public PageUrlProvider(IDocumentStore raven, IMessageAggregator messages, IPageUrlResolver pageUrlResolver, IApplicationContext context)
    {
      Raven = raven;
      Messages = messages;
      PageUrlResolver = pageUrlResolver;
      Context = context;
    }


    /// <inheritdoc />
    public async Task<PageRoute> Resolve(IRoute route)
    {
      PageRoute resolved = new PageRoute(route);

      List<string> ids = new List<string>();
      RouteReference reference = route.References.SingleOrDefault(x => x.Collection == PAGE_COLLECTION);

      ids.Add(reference.Id);
      ids.AddRange(route.Dependencies);

      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

      Dictionary<string, IPage> pages = await session.LoadAsync<IPage>(ids);

      if (!pages.TryGetValue(reference.Id, out IPage page) || page.AppId != route.AppId)
      {
        return null;
      }

      resolved.Page = page;
      resolved.Parents = pages.Where(x => x.Key != reference.Id).Select(x => x.Value).ToList();

      return resolved;
    }




    /// <inheritdoc />
    public async Task<IList<IRoute>> Run()
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

      List<IRoute> allRoutes = new List<IRoute>();
      IList<IPage> pages = await session.Query<IPage>().ToListAsync();
      IEnumerable<IGrouping<string, IPage>> groupedPages = pages.GroupBy(x => x.AppId);

      foreach (var group in groupedPages)
      {
        IList<IRoute> routes = TraversePageChildren(null, new List<IPage>() { }, group);
        allRoutes.AddRange(routes);
      }

      return allRoutes;
    }


    protected IRoute BuildRoute(IPage page, IEnumerable<IPage> parents)
    {
      IRoute route = new Route()
      {
        Id = String.Concat("routes.", IdGenerator.Create()),
        AppId = page.AppId,
        Url = PageUrlResolver.GetUrl(page, parents),
        ProviderAlias = Alias
      };

      route.Params.Add(PAGE_TYPE, page.PageTypeAlias);
      route.References.Add(new RouteReference(page.Id, PAGE_COLLECTION));

      if (parents != null)
      {
        foreach (IPage parent in parents)
        {
          route.Dependencies.Add(parent.Id);
        }
      }

      return route;
    }


    IList<IRoute> TraversePageChildren(IPage parent, IEnumerable<IPage> parents, IEnumerable<IPage> allPages)
    {
      List<IRoute> routes = new List<IRoute>();
      IEnumerable<IPage> currentPages = allPages.Where(x => x.ParentId == parent?.Id);

      foreach (IPage page in currentPages)
      {
        IRoute route = BuildRoute(page, parents);
        routes.Add(route);
        routes.AddRange(TraversePageChildren(page, parents.Union(new List<IPage>() { page }), allPages));
      }

      return routes;
    }
  }
}
