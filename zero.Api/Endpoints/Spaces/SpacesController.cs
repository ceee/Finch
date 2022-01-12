using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace zero.Api.Endpoints.Spaces;

public class SpacesController : ZeroApiEntityStoreController<Space, ISpaceStore>
{
  ISpaceTypeService SpaceTypes;

  public SpacesController(ISpaceStore store, ISpaceTypeService spaceTypes) : base(store)
  {
    SpaceTypes = spaceTypes;
  }


  [HttpGet("types")]
  [ZeroAuthorize(SpacePermissions.Read)]
  public virtual ActionResult<IEnumerable> GetTypes() => Ok(SpaceTypes.GetAll());

  [HttpGet("types/{alias}")]
  [ZeroAuthorize(SpacePermissions.Read)]
  public virtual ActionResult<SpaceType> GetType(string alias) => Ok(SpaceTypes.GetByAlias(alias));

  [HttpGet("{alias}")]
  [ZeroAuthorize(SpacePermissions.Read)]
  public virtual async Task<ActionResult<object>> Get(string alias, [FromQuery] ListQuery<Space> query = null)
  {
    SpaceType space = SpaceTypes.GetByAlias(alias);

    if (space == null)
    {
      return NotFound();
    }

    if (space.View != SpaceView.Editor)
    {
      query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
      query.SearchSelector ??= x => x.Name;
      Paged<Space> result = await Store.Load<zero_Api_Spaces_Listing>(query.Page, query.PageSize, q => q.Where(x => x.Flavor == alias).Filter(query));
      return result;
    }

    var stream = Store.Stream(q => q.Where(x => x.Flavor == alias));
    Space item = await stream.FirstOrDefaultAsync();
    return item;
  }


  [HttpGet("{alias}/empty")]
  [ZeroAuthorize(SpacePermissions.Create)]
  public virtual Task<ActionResult<Space>> Empty(string alias) => EmptyModel(alias);


  [HttpGet("{alias}/{id}")]
  [ZeroAuthorize(SpacePermissions.Read)]
  public virtual Task<ActionResult<Space>> Get(string alias, string id, string changeVector = null) => GetModel(id, changeVector);


  [HttpPost("")]
  [ZeroAuthorize(SpacePermissions.Create)]
  public virtual Task<ActionResult<Result>> Create([FromBody] Space saveModel) => CreateModel(saveModel);


  [HttpPut("{id}")]
  [ZeroAuthorize(SpacePermissions.Update)]
  public virtual Task<ActionResult<Result>> Update(string id, [FromBody] Space updateModel, [FromQuery] string changeToken = null) => UpdateModel(id, updateModel, changeToken);


  [HttpDelete("{id}")]
  [ZeroAuthorize(SpacePermissions.Delete)]
  public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}