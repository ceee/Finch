using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Web.Controllers
{
  public class PreviewController : BackofficeController
  {
    IPreviewApi Api;

    public PreviewController(IPreviewApi api)
    {
      Api = api;
    }


    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public async Task<IActionResult> Add([FromBody] IZeroEntity model)
    {
      EntityResult<IPreview> preview = await Api.Add(model);
      return Json(EntityResult<string>.From(preview, preview.Model?.Id));
    }


    public async Task<IActionResult> Update([FromBody] string id, [FromBody] IZeroEntity model)
    {
      EntityResult<IPreview> preview = await Api.Update(id, model);
      return Json(EntityResult<string>.From(preview, id));
    }
  }
}
