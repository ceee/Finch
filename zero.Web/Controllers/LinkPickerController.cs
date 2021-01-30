using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Web.Controllers
{
  public class LinkPickerController : BackofficeController
  {
    IPageTreeApi Api;
    IPagesApi PagesApi;

    public LinkPickerController(IPageTreeApi api, IPagesApi pagesApi)
    {
      Api = api;
      PagesApi = pagesApi;
    }


    public async Task<IList<TreeItem>> GetChildren([FromQuery] string areaAlias, [FromQuery] string parent = null, [FromQuery] string active = null)
    {
      return await Api.GetChildren(parent, active);
    }

    //public async Task<IList<PreviewModel>> GetPreviews([FromQuery] List<string> ids)
    //{
    //  return Previews(await PagesApi.GetByIds(ids.ToArray()), PreviewTransform);
    //}
  }
}
