using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.IO;

namespace zero.Api.Endpoints.Media;

/// <summary>
/// GET /empty
/// GET /
/// GET /{id}
/// GET /{id}/hierarchy
/// GET /{id}/{size}.tmp
/// POS /
/// PUT /{id}
/// PUT /{id/move/{folderId}
/// DEL /{id}
/// 
/// GET /folders/empty
/// GET /folders/{id}
/// GET /folders/{id}/children
/// GET /folders/{id}/hierarchy
/// POS /folders/
/// PUT /folders/{id}
/// PUT /folders/{id/move/{folderId}
/// DEL /folders/{id}
/// 
/// GET /bulk/descendants
/// PUT /bulk/move
/// DEL /bulk/delete
/// </summary>
public class MediaController : ZeroApiTreeEntityStoreController<zero.Media.Media, IMediaStore>
{
  IMediaManagement Media;

  public MediaController(IMediaStore store, IMediaManagement media) : base(store)
  {
    Media = media;
  }

  #region bulk operations

  [HttpPut("bulk/move")]
  [ZeroAuthorize(MediaPermissions.Update)]
  public virtual async Task<ActionResult<IEnumerable<Result>>> BulkMove(MediaBulkMoveOperation operation)
  {
    List<Result<string>> results = new();

    foreach (string id in operation.Ids)
    {
      Result<zero.Media.Media> result = await Store.Move(id, NormalizeParentId(operation.ParentId));
      results.Add(result.ConvertTo(id));
    }

    return results;
  }


  [HttpDelete("bulk/delete")]
  [ZeroAuthorize(MediaPermissions.Update)]
  public virtual async Task<ActionResult<IEnumerable<Result>>> BulkDelete(MediaBulkDeleteOperation operation)
  {
    List<Result<string>> results = new();

    foreach (string id in operation.Ids)
    {
      Result<string[]> result = await Store.DeleteWithDescendants(id);
      results.Add(result.ConvertTo(id));
    }

    return results;
  }


  [HttpGet("bulk/descendants")]
  [ZeroAuthorize(MediaPermissions.Update)]
  public virtual async Task<ActionResult<MediaDescendantStatistics>> GetDescendantStatistics([FromQuery] string[] ids)
  {
    List<Result<string>> results = new();

    Dictionary<string, zero.Media.Media> parents = await Store.Load(ids);

    int folderCount = parents.Count(x => x.Value != null && x.Value.IsFolder);
    int fileCount = parents.Count(x => x.Value != null && !x.Value.IsFolder);

    folderCount += await Store.Session.Query<MediaTreeHierarchyIndexResult, zero_Api_Media_Hierarchy>().CountAsync(x => x.Path.ContainsAny(ids) && x.IsFolder);
    fileCount += await Store.Session.Query<MediaTreeHierarchyIndexResult, zero_Api_Media_Hierarchy>().CountAsync(x => x.Path.ContainsAny(ids) && !x.IsFolder);

    return new MediaDescendantStatistics()
    {
      Files = fileCount,
      Folders = folderCount
    };
  }

  #endregion


  #region folder operations

  [HttpGet("folders/empty")]
  [ZeroAuthorize(MediaPermissions.Create)]
  public virtual async Task<ActionResult<zero.Media.Media>> EmptyFolder(string flavor = null) => await EmptyModel(flavor, x => x.IsFolder = true);


  [HttpGet("folders")]
  [ZeroAuthorize(MediaPermissions.Read)]
  public virtual Task<ActionResult<Paged>> GetFoldersByQuery([FromQuery] ListQuery<zero.Media.Media> query)
  {
    query.AdditionalQuery = q => q.Where(x => x.IsFolder);
    return GetModelsByIndex<MediaBasic, zero_Api_Media_Listing>(query);
  }


  [HttpGet("folders/{id}")]
  [ZeroAuthorize(MediaPermissions.Read)]
  public virtual async Task<ActionResult<zero.Media.Media>> GetFolder(string id, string changeVector = null) => await GetModel(id, changeVector);


  [HttpGet("folders/{id}/children")]
  [ZeroAuthorize(MediaPermissions.Read)]
  public virtual async Task<ActionResult<Paged>> GetFolderChildren(string id, [FromQuery] ListQuery<zero.Media.Media> query, [FromQuery] bool files = true)
  {
    id = NormalizeParentId(id);

    query.OrderQuery = q => q.OrderByDescending(x => x.IsFolder).ThenByDescending(x => x.CreatedDate);
    Paged<zero.Media.Media> result = await Store.LoadChildren<zero_Api_Media_Listing>(id, query.Page, query.PageSize, q => q.WhereIf(x => x.IsFolder, !files).Filter(query));
    Paged<MediaBasic> mappedResult = Mapper.Map<zero.Media.Media, MediaBasic>(result);

    // get children for all folders
    string[] folderIds = mappedResult.Items.Where(x => x.IsFolder).Select(x => x.Id).ToArray();
    IList<zero_Api_Media_ChildCounts.Result> children = await Store.Session.Query<zero_Api_Media_ChildCounts.Result, zero_Api_Media_ChildCounts>()
      .ProjectInto<zero_Api_Media_ChildCounts.Result>()
      .Where(x => x.Id.In(folderIds))
      .ToListAsync();

    foreach (MediaBasic item in mappedResult.Items)
    {
      if (item.IsFolder)
      {
        zero_Api_Media_ChildCounts.Result childCounts = children.FirstOrDefault(x => x.Id == item.Id);

        if (childCounts != null)
        {
          item.Children = !files ? childCounts.ChildFolderCount : childCounts.ChildCount;
        }
      }
    }

    return mappedResult;
  }


  [HttpGet("folders/{id}/hierarchy")]
  [ZeroAuthorize(MediaPermissions.Read)]
  public virtual async Task<ActionResult<zero.Media.Media[]>> GetFolderHierarchy(string id)
  {
    return await Store.GetHierarchy<zero_Api_Media_Hierarchy>(NormalizeParentId(id));
  }


  [HttpPost("folders")]
  [ZeroAuthorize(MediaPermissions.Create)]
  public virtual Task<ActionResult<Result>> CreateFolder(zero.Media.Media saveModel)
  {
    saveModel.IsFolder = true;
    return CreateModel(saveModel);
  }


  [HttpPut("folders/{id}")]
  [ZeroAuthorize(MediaPermissions.Update)]
  public virtual Task<ActionResult<Result>> UpdateFolder(string id, zero.Media.Media updateModel, [FromQuery] string changeToken = null) => UpdateModel(id, updateModel, changeToken);


  [HttpPut("folders/{id}/move/{destinationId}")]
  [ZeroAuthorize(MediaPermissions.Read)]
  public virtual async Task<ActionResult<Result>> MoveFolder(string id, string destinationId) => await MoveModel(id, NormalizeParentId(destinationId));


  [HttpDelete("folders/{id}")]
  [ZeroAuthorize(MediaPermissions.Delete)]
  public virtual Task<ActionResult<Result>> DeleteFolder(string id) => DeleteModelWithDescendants(id);

  #endregion


  #region file operations

  [HttpGet("empty")]
  [ZeroAuthorize(MediaPermissions.Create)]
  public virtual async Task<ActionResult<zero.Media.Media>> Empty(string flavor = null) => await EmptyModel(flavor, x => x.IsFolder = false);


  [HttpGet("")]
  [ZeroAuthorize(MediaPermissions.Read)]
  public virtual Task<ActionResult<Paged>> GetByQuery([FromQuery] ListQuery<zero.Media.Media> query)
  {
    query.AdditionalQuery = q => q.Where(x => !x.IsFolder);
    return GetModelsByIndex<MediaBasic, zero_Api_Media_Listing>(query);
  }


  [HttpGet("{id}")]
  [ZeroAuthorize(MediaPermissions.Read)]
  public virtual async Task<ActionResult<zero.Media.Media>> Get(string id, string changeVector = null) => await GetModel(id, changeVector);


  [HttpGet("{id}/hierarchy")]
  [ZeroAuthorize(MediaPermissions.Read)]
  public virtual async Task<ActionResult<zero.Media.Media[]>> GetHierarchy(string id) => await Store.GetHierarchy<zero_Api_Media_Hierarchy>(NormalizeParentId(id));


  [HttpPost("")]
  [ZeroAuthorize(MediaPermissions.Create)]
  public virtual async Task<ActionResult<Result>> Create([FromForm] IFormFile file, [FromForm] string folderId = null)
  {
    Result<zero.Media.Media> result = await Media.UploadFile(file, folderId);

    bool minimalResponse = Hints.ResponsePreference == ApiResponsePreference.Minimal;

    if (result.IsSuccess)
    {
      return Created("/", minimalResponse ? null : result.Model); // TODO correct URL
    }

    return result.WithoutModel();
  }


  [HttpPut("{id}")]
  [ZeroAuthorize(MediaPermissions.Update)]
  public virtual Task<ActionResult<Result>> Update(string id, zero.Media.Media updateModel, [FromQuery] string changeToken = null) => UpdateModel(id, updateModel, changeToken);


  [HttpPut("{id}/move/{destinationId}")]
  [ZeroAuthorize(MediaPermissions.Read)]
  public virtual async Task<ActionResult<Result>> Move(string id, string destinationId) => await MoveModel(id, NormalizeParentId(destinationId));


  [HttpDelete("{id}")]
  [ZeroAuthorize(MediaPermissions.Delete)]
  public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);


  [HttpGet("{id}/{size}.tmp")]
  public async Task<IActionResult> GetThumbnail(string id, string size)
  {
    zero.Media.Media media = await Media.GetFile(id);

    if (media == null)
    {
      return NotFound();
    }

    string path = Media.GetPublicFilePath(media);

    if (path == null)
    {
      return NotFound();
    }

    if (path.StartsWith("url://"))
    {
      path = path.Substring(6);
    }

    FileExtensionContentTypeProvider provider = new();
    string contentType;
    if (!provider.TryGetContentType(Path.GetFileName(path), out contentType))
    {
      contentType = "application/octet-stream";
    }

    try
    {
      return File(await Media.GetFileStream(media), contentType, DateTimeOffset.Now, EntityTagHeaderValue.Any);
    }
    catch (FileSystemException)
    {
      return NotFound();
    }
  }

  #endregion
}