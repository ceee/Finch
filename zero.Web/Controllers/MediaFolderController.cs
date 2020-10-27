using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

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


    public async Task<EditModel<IMediaFolder>> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public EditModel<IMediaFolder> GetEmpty([FromServices] IMediaFolder blueprint) => Edit(blueprint);


    public async Task<IList<IMediaFolder>> GetHierarchy([FromQuery] string id) => await Api.GetHierarchy(id);


    public async Task<IList<TreeItem>> GetAllAsTree([FromQuery] string parent = null, [FromQuery] string active = null) => await Api.GetAllAsTree(parent, active);


    [HttpPost]
    public async Task<EntityResult<IMediaFolder>> Move([FromBody] ActionCopyModel model) => await Api.Move(model.Id, model.DestinationId);


    public async Task<EntityResult<IMediaFolder>> Save([FromBody] IMediaFolder model) => await Api.Save(model);


    public async Task<EntityResult<IMediaFolder>> Delete([FromQuery] string id) => await Api.Delete(id);
  }
}
