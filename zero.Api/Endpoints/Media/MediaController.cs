using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Media;

public class MediaController : ZeroApiTreeEntityStoreController<zero.Media.Media, IMediaStore>
{
  public MediaController(IMediaStore store) : base(store)
  {

  }


  [HttpGet("empty")]
  [ZeroAuthorize(MediaPermissions.Create)]
  public virtual async Task<ActionResult<MediaEdit>> Empty(MediaType type, string flavor = null) => await EmptyModel<MediaEdit>(flavor, x => x.Type = type);


  [HttpGet("{id}")]
  [ZeroAuthorize(MediaPermissions.Read)]
  public virtual async Task<ActionResult<MediaEdit>> Get(string id, string changeVector = null)
  {
    zero.Media.Media model = await Store.Load(id, changeVector);

    if (model == null)
    {
      return NotFound();
    }

    HttpContext.Items[ApiConstants.ChangeVector] = Store.GetChangeToken(model);

    if (model.Type == MediaType.Folder)
    {
      return Mapper.Map<zero.Media.Media, MediaFolderEdit>(model);
    }

    return Mapper.Map<zero.Media.Media, MediaFileEdit>(model);
  }


  //[HttpGet("")]
  //[ZeroAuthorize(MediaPermissions.Read)]
  //public virtual Task<ActionResult<Paged>> Get([FromQuery] ListQuery<zero.Media.Media> query)
  //{
  //  query.OrderQuery = q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name);
  //  return GetModels<MediaBasic, zero_Api_Media_Listing>(query);
  //}


  //[HttpPost("")]
  //[ZeroAuthorize(MediaPermissions.Create)]
  //public virtual Task<ActionResult<Result>> Create(MediaSave saveModel) => CreateModel<MediaSave, MediaEdit>(saveModel);


  //[HttpPut("{id}")]
  //[ZeroAuthorize(MediaPermissions.Update)]
  //public virtual Task<ActionResult<Result>> Update(string id, MediaSave updateModel, [FromQuery] string changeToken = null) => UpdateModel<MediaSave, MediaEdit>(id, updateModel, changeToken);


  //[HttpDelete("{id}")]
  //[ZeroAuthorize(MediaPermissions.Delete)]
  //public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}