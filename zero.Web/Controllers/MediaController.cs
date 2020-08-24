using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
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

    IMediaUploadApi MediaUploadApi;


    public MediaController(IMediaApi api, IMediaFolderApi mediaFolderApi, IMediaUploadApi mediaUploadApi)
    {
      Api = api;
      MediaFolderApi = mediaFolderApi;
      MediaUploadApi = mediaUploadApi;
    }


    /// <summary>
    /// Get media item by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      return Edit(await Api.GetById(id));
    }


    /// <summary>
    /// Get media item by id
    /// </summary>  
    public async Task<IActionResult> GetByIds([FromQuery] string[] ids)
    {
      return Json(await Api.GetById(ids));
    }


    /// <summary>
    /// Get all media items + folders
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] MediaListQuery query)
    {
      ListResult<MediaListModel> items = await Mapper.Map<Media, MediaListModel>(await Api.GetByQuery(query));
      IList<MediaFolder> hierarchy = null;
      IEnumerable<MediaListModel> folders = new List<MediaListModel>();
      MediaFolder folder = null;

      if (query.Page < 2)
      {
        folders = await Mapper.Map<MediaFolder, MediaListModel>(await MediaFolderApi.GetAll(query.FolderId));
      }

      if (!String.IsNullOrEmpty(query.FolderId))
      {
        folder = await MediaFolderApi.GetById(query.FolderId);
        hierarchy = await MediaFolderApi.GetHierarchy(query.FolderId);
      }   

      return Json(new MediaListResultModel(items, folders, folder, hierarchy));
    }


    /// <summary>
    /// Save a media item
    /// </summary>
    public async Task<IActionResult> Save([FromBody] Media model)
    {
      return Json(await Api.Save(model));
    }


    /// <summary>
    /// Upload a file
    /// </summary>
    public async Task<IActionResult> Upload(IFormFile file, string folderId)
    {
      Media media = await MediaUploadApi.Upload(file, folderId);
      return Json(await Api.Save(media));
    }


    /// <summary>
    /// Upload a file
    /// </summary>
    public async Task<IActionResult> UploadTemporary(IFormFile file, string folderId)
    {
      Media media = await MediaUploadApi.Upload(file, folderId);
      return Json(media);
    }


    /// <summary>
    /// Deletes a media item
    /// </summary>
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return await As<Media, MediaEditModel>(await Api.Delete(id));
    }


    /// <summary>
    /// Streams a media thumbnail by id
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> StreamThumbnail(string id)
    {
      string path = await Api.GetSourceById(id, true);

      if (path == null)
      {
        return NotFound();
      }

      var provider = new FileExtensionContentTypeProvider();
      string contentType;
      if (!provider.TryGetContentType(Path.GetFileName(path), out contentType))
      {
        contentType = "application/octet-stream";
      }

      return File(path, contentType);
    }
  }
}
