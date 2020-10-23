using Raven.Client.Documents;
using Raven.Client.Documents.Commands.Batches;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Entities.Messages;
using zero.Core.Extensions;
using zero.Core.Messages;
using zero.Core.Utils;

namespace zero.Core.Routing
{
  public class PageUrlProviderTest
  {
    public string Alias => "zero.pages";

    public const string PAGE_COLLECTION = "page";

    public const string PAGE_TYPE = "pageType";

    protected IDocumentStore Raven { get; private set; }

    protected IMessageAggregator Messages { get; private set; }

    protected IPageUrlResolver PageUrlResolver { get; set; }

    protected IApplicationContext Context { get; private set; }


    public PageUrlProviderTest(IDocumentStore raven, IMessageAggregator messages, IPageUrlResolver pageUrlResolver, IApplicationContext context)
    {
      Raven = raven;
      Messages = messages;
      PageUrlResolver = pageUrlResolver;
      Context = context;
    }


    /// <inheritdoc />
    public bool UrlChanged(IPage previous, IPage current)
    {
      return previous == null || !previous.Alias.Equals(current.Alias) || !previous.ParentId.Equals(current.ParentId);
    }


    /// <inheritdoc />
    public Task GetDependencies(IPage model)
    {
      // TODO get all pages (and other routes) which have this page as a dependency and update them
      throw new NotImplementedException();
    }


    /// <inheritdoc />
    public async Task<int> UpdateAll()
    {
      throw new NotImplementedException();

      //using IAsyncDocumentSession session = Raven.OpenAsyncSession();

      //IList<IPage> pages = await session.Query<IPage>().ToListAsync();
      //IEnumerable<IGrouping<string, IPage>> groupedPages = pages.GroupBy(x => x.AppId);

      //Dictionary<string, UrlRoute> map = new Dictionary<string, UrlRoute>();
      //HashSet<PatchCommandData> commands = new HashSet<PatchCommandData>();

      //async Task traversePageChildren(IPage parent, IEnumerable<IPage> parents, IEnumerable<IPage> allPages)
      //{
      //  IEnumerable<IPage> currentPages = allPages.Where(x => x.ParentId == parent?.Id);

      //  foreach (IPage page in currentPages)
      //  {
      //    UrlRoute route = new UrlRoute()
      //    {
      //      Url = GetUrl(page, parents),
      //      Dependencies = parents.Select(x => x.Id).ToArray()
      //    };

      //    commands.Add(new PatchCommandData(
      //      id: page.Id,
      //      changeVector: null,
      //      patchIfMissing: null,
      //      patch: new PatchRequest()
      //      {
      //        Values = { { "route", route } },
      //        Script = "this.Route = args.route"
      //      }
      //    ));

      //    await traversePageChildren(page, parents.Union(new List<IPage>() { page }), allPages);
      //  }
      //};

      //foreach (var group in groupedPages)
      //{
      //  await traversePageChildren(null, new List<IPage>() { }, group);
      //}

      //using (IAsyncDocumentSession commandSession = Raven.OpenAsyncSession())
      //{
      //  commandSession.Advanced.Defer(commands.ToArray());
      //  await commandSession.SaveChangesAsync();
      //}

      //return commands.Count;
    }


    /// <summary>
    /// Create url for page 
    /// </summary>
    protected string GetUrl(IPage page, IEnumerable<IPage> parents)
    {
      StringBuilder stringBuilder = new StringBuilder();

      stringBuilder.Append('/');

      if (parents != null)
      {
        foreach (IPage parent in parents)
        {
          stringBuilder.Append(parent.Alias);
          stringBuilder.Append('/');
        }
      }

      stringBuilder.Append(page.Alias);

      return stringBuilder.ToString();
    }
  }
}
