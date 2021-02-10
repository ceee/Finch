using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Routing
{
  public class PageLinkProvider : ILinkProvider
  {
    protected IRoutes Routes { get; set; }
    protected IZeroOptions Options { get; set; }

    public PageLinkProvider(IRoutes routes, IZeroOptions options)
    {
      Routes = routes;
      Options = options;
    }


    /// <inheritdoc />
    public bool CanProcess(ILink link) => link.Area == "zero.pages";


    /// <inheritdoc />
    public async Task<string> Resolve(ILink link)
    {
      return await Routes.GetUrl<IPage>(link.Values.GetValueOrDefault<string>("id"));
    }


    /// <inheritdoc />
    public async Task<PreviewModel> Preview(IAsyncDocumentSession session, ILink link)
    {
      string id = link.Values.GetValueOrDefault<string>("id");

      if (id.IsNullOrEmpty())
      {
        return null;
      }

      IPage page = await session.LoadAsync<IPage>(id);

      if (page == null)
      {
        return null;
      }

      PageType pageType = Options.Pages.GetAllItems().FirstOrDefault(x => x.Alias == page.PageTypeAlias);

      string url = await Routes.GetUrl<IPage>(page);

      return new()
      {
        Id = page.Id,
        Icon = pageType?.Icon ?? "fth-folder",
        Name = page.Name,
        Text = url
      };
    }
  }
}
