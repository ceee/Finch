using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Entities.Messages;
using zero.Core.Messages;
using zero.Core.Utils;

namespace zero.Core.Routing
{
  public class InternalUrlProvider : IUrlProvider
  {
    public const string PAGE_COLLECTION = "page";

    public const string PAGE_TYPE = "pageType";

    protected IDocumentStore Raven { get; private set; }

    protected IMessageAggregator Messages { get; private set; }

    protected IPageUrlResolver PageUrlResolver { get; set; }


    protected IApplicationContext Context { get; private set; }

    public InternalUrlProvider(IDocumentStore raven, IMessageAggregator messages, IPageUrlResolver pageUrlResolver, IApplicationContext context)
    {
      Raven = raven;
      Messages = messages;
      PageUrlResolver = pageUrlResolver;
      Context = context;
    }


    /// <inheritdoc />
    public void Attach()
    {
      //Messages.Subscribe<EntitySavedMessage, 
    }


    /// <inheritdoc />
    public async Task Rebuild()
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

      IList<IPage> pages = await session.Query<IPage>().ToListAsync();
      IEnumerable<IGrouping<string, IPage>> groupedPages = pages.GroupBy(x => x.AppId);

      foreach (var group in groupedPages)
      {
        IApplicationContext context = await Context.ForId(group.Key);

        foreach (IPage page in group.Where(x => x.ParentId == null))
        {
          PageUrlResolver.GetUrl(context, page, null);
        }
      }
    }


    protected IRoute BuildRoute(IApplicationContext context, IPage page, IEnumerable<IPage> parents)
    {
      UrlInfo info = PageUrlResolver.GetUrl(context, page, parents);

      IRoute route = new Route()
      {
        Id = String.Concat("routes.", IdGenerator.Create()),
        AppId = context.AppId,
        Url = info.IsUrl ? info.Text : null
      };

      route.Params.Add(PAGE_TYPE, page.PageTypeAlias);
      route.References.Add(new RouteReference(page.Id, PAGE_COLLECTION));

      return route;
    }


    //void TraversePageChildren(IPage parent, IList<IPage> pages)
    //{
    //  foreach (IPage page in pages.Where(x => x.ParentId == parent?.Id))
    //  {
    //    PageUrlResolver.GetUrl(context, page, null);
    //  }
    //}
  }
}
