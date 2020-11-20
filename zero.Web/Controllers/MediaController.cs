using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Sections.Media, PermissionsValue.True)]
  public class MediaController : BackofficeCollectionController<IMedia, IMediaCollection>
  {
    IMediaFolderApi MediaFolderApi;
    IPaths Paths;

    public MediaController(IMediaCollection collection, IMediaFolderApi mediaFolderApi, IPaths paths) : base(collection)
    {
      MediaFolderApi = mediaFolderApi;
      Paths = paths;
      PreviewTransform = (item, model) => model.Icon = "fth-globe";
    }


    public async Task<ListResult<MediaListItem>> GetListByQuery([FromQuery] MediaListItemQuery query) => await Collection.GetListByQuery(query);


    [HttpPost]
    public async Task<EntityResult<IMedia>> Upload(IFormFile file, string folderId) => await Collection.Save(await Collection.Upload(file, folderId));

    [HttpPost]
    public async Task<Media> UploadTemporary(IFormFile file, string folderId) => await Collection.Upload(file, folderId);

    [HttpPost]
    public async Task<EntityResult<IMedia>> Move([FromBody] ActionCopyModel model) => await Collection.Move(model.Id, model.DestinationId);


    public async Task<MediaListResultModel> GetAll([FromQuery] MediaListQuery query)
    {
      ListResult<MediaListModel> items = (await Collection.GetByQuery(query)).MapTo(x => new MediaListModel()
      {
        Id = x.Id,
        IsFolder = false,
        Name = x.Name,
        Size = x.Size,
        Source = x.PreviewSource ?? x.Source,
        Type = x.Type
      });

      IList<IMediaFolder> hierarchy = null;
      IMediaFolder folder = null;

      if (!String.IsNullOrEmpty(query.FolderId))
      {
        folder = await MediaFolderApi.GetById(query.FolderId);
        hierarchy = await MediaFolderApi.GetHierarchy(query.FolderId);
      }   

      return new MediaListResultModel(items, null, folder, hierarchy);
    }


    [HttpGet]
    public async Task<IActionResult> StreamThumbnail([FromQuery] string id, [FromQuery] bool thumb = true)
    {
      string path = await Collection.GetSourceById(id, thumb);

      if (path == null)
      {
        return NotFound();
      }

      var provider = new FileExtensionContentTypeProvider();
      string contentType;
      if (path == null || !provider.TryGetContentType(Path.GetFileName(path), out contentType))
      {
        contentType = "application/octet-stream";
      }

      if (path.StartsWith("http"))
      {
        using var client = new WebClient();
        var content = client.DownloadData(path);
        return File(content, contentType, DateTimeOffset.Now, EntityTagHeaderValue.Any);
      }

      string fullPath = Path.Combine(Paths.Root, path?.Trim('/') ?? String.Empty);

      if (path == null || !System.IO.File.Exists(fullPath))
      {
        return NotFound();
      }

      return File(path, contentType, DateTimeOffset.Now, EntityTagHeaderValue.Any);
    }
  }
}
