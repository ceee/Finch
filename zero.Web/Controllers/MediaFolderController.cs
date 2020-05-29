using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Options;
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


    /// <summary>
    /// Get media folder by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      return await As<MediaFolder, MediaFolderEditModel>(await Api.GetById(id));
    }


    /// <summary>
    /// Get empty media folder model
    /// </summary>  
    public IActionResult GetEmpty()
    {
      return Json(new MediaFolderEditModel());
    }


    /// <summary>
    /// Get all folders with a specific parent as tree
    /// </summary>    
    public async Task<IActionResult> GetAllAsTree([FromQuery] string parent = null, [FromQuery] string active = null)
    {
      return Json(await Api.GetAllAsTree(parent, active));
    }


    /// <summary>
    /// Save a media item
    /// </summary>
    public async Task<IActionResult> Save([FromBody] MediaFolderEditModel model)
    {
      MediaFolder entity = await Mapper.Map(model, await Api.GetById(model.Id));
      return await As<MediaFolder, MediaFolderEditModel>(await Api.Save(entity));
    }


    /// <summary>
    /// Deletes a media item
    /// </summary>
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return await As<MediaFolder, MediaFolderEditModel>(await Api.Delete(id));
    }
  }
}
