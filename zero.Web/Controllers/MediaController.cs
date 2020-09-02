using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
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

  
    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public async Task<IActionResult> GetByIds([FromQuery] string[] ids) => Json(await Api.GetById(ids));


    public async Task<IActionResult> GetListByQuery([FromQuery] MediaListItemQuery query) => Json(await Api.GetListByQuery(query));


    public async Task<IActionResult> Save([FromBody] IMedia model) => Json(await Api.Save(model));


    public async Task<IActionResult> Upload(IFormFile file, string folderId) => Json(await Api.Save(await Api.Upload(file, folderId)));


    public async Task<IActionResult> UploadTemporary(IFormFile file, string folderId) => Json(await Api.Upload(file, folderId));


    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));


    public async Task<IActionResult> GetAll([FromQuery] MediaListQuery query)
    {
      ListResult<MediaListModel> items = await Mapper.Map<IMedia, MediaListModel>(await Api.GetByQuery(query));
      IList<IMediaFolder> hierarchy = null;
      IEnumerable<IMediaFolder> folders = new List<IMediaFolder>();
      IMediaFolder folder = null;

      if (query.Page < 2)
      {
        folders = await MediaFolderApi.GetAll(query.FolderId);
      }

      if (!String.IsNullOrEmpty(query.FolderId))
      {
        folder = await MediaFolderApi.GetById(query.FolderId);
        hierarchy = await MediaFolderApi.GetHierarchy(query.FolderId);
      }   

      return Json(new MediaListResultModel(items, null, folder, hierarchy));
    }


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
