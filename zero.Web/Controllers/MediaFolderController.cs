using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Sections.Media, PermissionsValue.True)]
  public class MediaFolderController : BackofficeController
  {
    IMediaFolderApi Api;

    public MediaFolderController(IMediaFolderApi api)
    {
      Api = api;
    }


    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public IActionResult GetEmpty([FromServices] IMediaFolder blueprint) => Edit(blueprint);


    public async Task<IActionResult> GetHierarchy([FromQuery] string id) => Json(await Api.GetHierarchy(id));


    public async Task<IActionResult> GetAllAsTree([FromQuery] string parent = null, [FromQuery] string active = null) => Json(await Api.GetAllAsTree(parent, active));


    public async Task<IActionResult> Save([FromBody] IMediaFolder model) => Json(await Api.Save(model));


    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
