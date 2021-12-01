using Microsoft.AspNetCore.Mvc;
using zero.Api.Controllers;

namespace zero.Backoffice.Endpoints.Links;

public class LinksController : ZeroApiController
{
  readonly IZeroStore Store;
  readonly ILinks Links;

  public LinksController(IZeroStore store, ILinks links)
  {
    Store = store;
    Links = links;
  }

  [HttpPost("convert")]
  //[ZeroAuthorize(CountryPermissions.Create)]
  public async Task<IList<LinkPreview>> ConvertToLinks([FromBody] List<Link> links)
  {
    IZeroDocumentSession session = Store.Session();
    List<LinkPreview> previews = new();

    foreach (Link link in links)
    {
      ILinkProvider provider = Links.GetProvider(link);
      LinkPreview model = null;

      if (provider != null)
      {
        model = await provider.Preview(session, link);
      }

      previews.Add(model ?? new LinkPreview()
      {
        HasError = true,
        Icon = "fth-alert-circle color-red",
        Id = "tmp_" + IdGenerator.Create(),
        Name = "@errors.preview.notfound",
        Text = "@errors.preview.notfound_text"
      });
    }

    return previews;
  }
}