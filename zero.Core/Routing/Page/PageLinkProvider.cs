using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class PageLinkProvider : ILinkProvider
  {
    /// <inheritdoc />
    public string Name { get; } = "@links.providers.page";

    /// <inheritdoc />
    public string Alias { get; } = "zero.pages";

    protected IZeroStore Store { get; set; }

    protected IRoutes Routes { get; set; }


    public PageLinkProvider(IZeroStore store, IRoutes routes)
    {
      Store = store;
      Routes = routes;
    }


    /// <inheritdoc />
    public async Task<string> ResolveLink(ILink link)
    {
      if (!link.Values.TryGetValue("pageId", out object pageIdObj))
      {
        return null;
      }

      string pageId = pageIdObj.ToString();

      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      IPage page = await session.LoadAsync<IPage>(pageId);
      IRoute route = await Routes.GetRoute(page);

      return route.Url;
    }
  }
}
