using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Options;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Sections.Media, PermissionsValue.True)]
  public class MediaController : BackofficeController
  {
    IMediaApi Api;

    IMediaFolderApi MediaFolderApi;


    public MediaController(IMediaApi api, IMediaFolderApi mediaFolderApi)
    {
      Api = api;
      MediaFolderApi = mediaFolderApi;
    }


    /// <summary>
    /// Get media item by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      return await As<Media, MediaEditModel>(await Api.GetById(id));
    }


    /// <summary>
    /// Get all media items + folders
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] MediaListQuery query)
    {
      ListResult<MediaListModel> items = await Mapper.Map<Media, MediaListModel>(await Api.GetByQuery(query));
      IEnumerable<MediaListModel> folders = new List<MediaListModel>();

      if (query.Page < 2)
      {
        folders = await Mapper.Map<MediaFolder, MediaListModel>(await MediaFolderApi.GetAll(query.FolderId));
      }

      return Json(new MediaListResultModel(items, folders));
    }


    /// <summary>
    /// Save a media item
    /// </summary>
    public async Task<IActionResult> Save([FromBody] MediaEditModel model)
    {
      Media entity = await Mapper.Map(model, await Api.GetById(model.Id));
      return await As<Media, MediaEditModel>(await Api.Save(entity));
    }


    /// <summary>
    /// Deletes a media item
    /// </summary>
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return await As<Media, MediaEditModel>(await Api.Delete(id));
    }
  }
}
