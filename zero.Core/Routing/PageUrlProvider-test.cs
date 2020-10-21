using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
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
    public async Task<IRoute> CreateRoute(IPage model)
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

      Pages_ByHierarchy.Result result = await session.Query<Pages_ByHierarchy.Result, Pages_ByHierarchy>()
        .ProjectInto<Pages_ByHierarchy.Result>()
        .Include<Pages_ByHierarchy.Result, IPage>(x => x.Path.Select(p => p.Id))
        .FirstOrDefaultAsync(x => x.Id == model.Id);

      IList<IPage> parents = (await session.LoadAsync<IPage>(result.Path.Select(x => x.Id))).Select(x => x.Value).ToList();

      return new Route()
      {
        Id = String.Concat("routes.", IdGenerator.Create()),
        AppId = model.AppId,
        Url = GetUrl(model, parents),
        ProviderAlias = Alias
      };
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
