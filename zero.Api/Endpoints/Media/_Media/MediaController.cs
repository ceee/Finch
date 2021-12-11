//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.StaticFiles;
//using Microsoft.Net.Http.Headers;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net.Http;
//using System.Threading.Tasks;
//using zero.Core;
//using zero.Core.Collections;
//using zero.Core.Entities;
//using zero.Core.Identity;
//using zero.Web.Models;

//namespace zero.Web.Controllers
//{
//  [ZeroAuthorize(Permissions.Sections.Media, PermissionsValue.True)]
//  public class MediaController : ZeroBackofficeCollectionController<Media, IMediaCollection>
//  {
//    IPaths Paths;

//    public MediaController(IMediaCollection collection, IPaths paths) : base(collection)
//    {
//      Paths = paths;
//      PreviewTransform = (item, model) => model.Icon = "fth-globe";
//    }


//    public async Task<Paged<MediaListItem>> GetListByQuery([FromQuery] MediaListItemQuery query)
//    {
//      query.IncludeInactive = true;
//      return await Collection.Load(query);
//    }


//    [HttpPost]
//    public async Task<Result<Media>> Upload(IFormFile file, [FromForm] string folderId) => await Collection.Save(await Collection.Upload(file, folderId));

//    [HttpPost]
//    public async Task<Media> UploadTemporary(IFormFile file, [FromForm] string folderId) => await Collection.Upload(file, folderId);

//    [HttpPost]
//    public async Task<Result<Media>> Move([FromBody] ActionCopyModel model) => await Collection.Move(model.Id, model.DestinationId);


//    public async Task<MediaListResultModel> GetAll([FromQuery] MediaListQuery query)
//    {
//      query.IncludeInactive = true;
//      Paged<MediaListModel> items = (await Collection.Load(query)).MapTo(x => new MediaListModel()
//      {
//        Id = x.Id,
//        IsFolder = false,
//        Name = x.Name,
//        Size = x.Size,
//        Source = x.PreviewSource ?? x.Source,
//        Type = x.Type
//      });

//      IList<MediaFolder> hierarchy = null;
//      MediaFolder folder = null;

//      if (!String.IsNullOrEmpty(query.FolderId))
//      {
//        folder = await Collection.Folders.Load(query.FolderId);
//        hierarchy = await Collection.Folders.GetHierarchy(query.FolderId);
//      }   

//      return new MediaListResultModel(items, null, folder, hierarchy);
//    }
//  }
//}
