using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Sections.Media, PermissionsValue.True)]
  public class MediaFolderController : ZeroBackofficeCollectionController<MediaFolder, IMediaFolderCollection>
  {
    public MediaFolderController(IMediaFolderCollection collection) : base(collection) { }


    public async Task<IList<MediaFolder>> GetHierarchy([FromQuery] string id) => await Collection.GetHierarchy(id);


    public async Task<IList<TreeItem>> GetAllAsTree([FromQuery] string parent = null, [FromQuery] string active = null) => await Collection.LoadAsTree(parent, active);


    [HttpPost]
    public async Task<EntityResult<MediaFolder>> Move([FromBody] ActionCopyModel model) => await Collection.Move(model.Id, model.DestinationId);
  }
}
