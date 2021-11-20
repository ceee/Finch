using Raven.Client.Documents.Session;

namespace zero.Routing;

public class PageLinkProvider : ILinkProvider
{
  public const string AREA = "zero.pages";

  protected IRoutes Routes { get; set; }
  protected IZeroOptions Options { get; set; } 

  public PageLinkProvider(IRoutes routes, IZeroOptions options)
  {
    Routes = routes;
    Options = options;
  }


  /// <summary>
  /// Creates a new link object from a page
  /// </summary>
  public Link Create(Page page, LinkTarget target = LinkTarget.Default, string title = null)
  {
    return new Link()
    {
      Area = AREA,
      Target = target,
      Title = title,
      Values = new()
      {
        { "id", page.Id }
      }
    };
  }


  /// <inheritdoc />
  public bool CanProcess(Link link) => link.Area == AREA;


  /// <inheritdoc />
  public async Task<string> Resolve(Link link)
  {
    link.Url = await Routes.GetUrl<Page>(link.Values.GetValueOrDefault<string>("id")) + (link.UrlSuffix ?? string.Empty);
    return link.Url;
  }


  /// <inheritdoc />
  public async Task<LinkPreview> Preview(IAsyncDocumentSession session, Link link)
  {
    string id = link.Values.GetValueOrDefault<string>("id");

    if (id.IsNullOrEmpty())
    {
      return null;
    }

    Page page = await session.LoadAsync<Page>(id);

    if (page == null)
    {
      return null;
    }

    PageType pageType = Options.Pages.GetAllItems().FirstOrDefault(x => x.Alias == page.PageTypeAlias);

    string url = await Routes.GetUrl<Page>(page);

    return new()
    {
      Id = page.Id,
      Icon = pageType?.Icon ?? "fth-folder",
      Name = page.Name,
      Text = url
    };
  }
}
