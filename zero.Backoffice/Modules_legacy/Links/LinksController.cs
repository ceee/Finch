using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Routing;
using zero.Core.Utils;

namespace zero.Web.Controllers
{
  public class LinksController : BackofficeController
  {
    IZeroStore Store;
    ILinks Links;

    public LinksController(IZeroStore store, ILinks links)
    {
      Store = store;
      Links = links;
    }

    [HttpPost]
    public async Task<IList<PickerPreviewModel>> GetPreviews([FromBody] List<Link> links)
    {
      IZeroDocumentSession session = Store.Session();
      IList<PickerPreviewModel> previews = new List<PickerPreviewModel>();

      foreach (Link link in links)
      {
        ILinkProvider provider = Links.GetProvider(link);
        PickerPreviewModel model = null;

        if (provider != null)
        {
          model = await provider.Preview(session, link);
        }

        previews.Add(model ?? new PickerPreviewModel()
        {
          HasError = true,
          Icon = "fth-alert-circle color-red",
          Id = IdGenerator.Create(),
          Name = "@errors.preview.notfound",
          Text = "@errors.preview.notfound_text"
        });
      }

      return previews;
    }
  }
}
