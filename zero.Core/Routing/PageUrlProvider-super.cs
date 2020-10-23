using Microsoft.Extensions.Logging;
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
  public class PageUrlProviderSuper : IUrlProviderSuper
  {
    public string Alias => "zero.pages";

    public const string PAGE_COLLECTION = "page";

    public const string PAGE_TYPE = "pageType";

    protected IDocumentStore Raven { get; private set; }

    protected IMessageAggregator Messages { get; private set; }

    protected IPageUrlResolver PageUrlResolver { get; set; }

    public PageUrlProviderSuper(IDocumentStore raven, IMessageAggregator messages, IPageUrlResolver pageUrlResolver)
    {
      Raven = raven;
      Messages = messages;
      PageUrlResolver = pageUrlResolver;
    }

    public Task<IResolvedRoute> ResolveRoute(IAsyncDocumentSession session, IRoute route)
    {
      throw new NotImplementedException();
    }
  }


  public interface IUrlProviderSuper
  {
    string Alias { get; }
    Task<IResolvedRoute> ResolveRoute(IAsyncDocumentSession session, IRoute route);
  }


  public class Test
  {
    IEnumerable<IUrlProviderSuper> Providers;
    IDocumentStore Raven;
    ILogger<Test> Logger;

    public Test()
    {

    }


    public async Task<IResolvedRoute> ResolveUrl(string appId, string path)
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

      IList<IRoute> routes = await session.Query<IRoute>()
        .Where(x => x.AppId == appId)
        .Where(x => (!x.AllowSuffix && x.Url == path) || (x.AllowSuffix && path.StartsWith(x.Url)))
        .ToListAsync();

      if (routes.Count > 1)
      {
        Logger.LogWarning("Multiple routes {routes} were found for {path}", routes.Select(x => x.Id), path);
      }
      else if (routes.Count < 1)
      {
        return null;
      }

      IRoute route = routes.First();
      IUrlProviderSuper provider = Providers.FirstOrDefault(x => x.Alias == route.ProviderAlias);

      if (provider == null)
      {
        Logger.LogWarning("Could not locate URL provider {provider}", route.ProviderAlias);
        return null;
      }

      return await provider.ResolveRoute(session, route);
    }
  }
}
