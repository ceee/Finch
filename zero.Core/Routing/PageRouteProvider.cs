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

    protected ILogger<PageRouteProvider> Logger { get; set; }


    public PageRouteProvider(ILogger<PageRouteProvider> logger) : base("zero.pages")
    {
      Logger = logger;
    }


    public string GetRouteId(IPage model)
    {
      return Alias + "." + model.Hash;
    }


    /// <inheritdoc />
    public override async Task<IRoute> GetRoute(IAsyncDocumentSession session, IPage model)
    {
      IList<IRoute> routes = await session.Query<IRoute>()
        .Where(x => x.AppId == model.AppId && x.ProviderAlias == Alias && x.References[0].Collection == REF_KEY && x.References[0].Id == model.Id)
        .ToListAsync();

      if (routes.Count > 1)
      {
        Logger.LogWarning("Multiple routes {routes} were found for page {id}", routes.Select(x => x.Id), model.Id);
      }
      else if (routes.Count < 1)
      {
        return null;
      }

      return routes.First();
    }


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

      if (!pages.TryGetValue(reference.Id, out IPage page) || page.AppId != route.AppId)
      {
        return null;
      }

      resolved.Page = page;
      resolved.Parents = pages.Where(x => x.Key != reference.Id).Select(x => x.Value).ToList();

      return resolved;
    }
  }
}
