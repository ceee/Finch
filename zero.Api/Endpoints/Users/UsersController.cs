using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Users;

public class UsersController : ZeroApiController
{
  readonly IUserService Users;

  public UsersController(IUserService users)
  {
    Users = users;
  }


  [HttpGet("empty")]
  [ZeroAuthorize(UserPermissions.Create)]
  public virtual ActionResult<ZeroUser> Empty(string flavor = null)
  {
    return new ZeroUser();
  }


  [HttpGet("{id}")]
  [ZeroAuthorize(UserPermissions.Read)]
  public virtual async Task<ActionResult<ZeroUser>> Get(string id)
  {
    ZeroUser model = await Users.GetUserById(id);

    if (model == null)
    {
      return NotFound();
    }

    //HttpContext.Items[ApiConstants.ChangeToken] = Store.GetChangeToken(model);

    return model;
  }


  [HttpGet("")]
  [ZeroAuthorize(UserPermissions.Read)]
  public virtual async Task<ActionResult<Paged>> Get([FromQuery] ListQuery<ZeroUser> query)
  {
    query.OrderQuery ??= q => q.OrderByDescending(x => x.CreatedDate);
    query.SearchSelector ??= x => x.Name;
    Paged<ZeroUser> result = await Users.GetAll(query.Page, query.PageSize);
    return Mapper.Map<ZeroUser, UserBasic>(result);
  }


  //[HttpPost("")]
  //[ZeroAuthorize(UserPermissions.Create)]
  //public virtual Task<ActionResult<Result>> Create(ZeroUser saveModel) => CreateModel(saveModel);


  [HttpPut("{id}")]
  [ZeroAuthorize(UserPermissions.Update)]
  public virtual async Task<ActionResult<Result>> Update(string id, ZeroUser updateModel, [FromQuery] string changeToken = null)
  {
    if (id != updateModel.Id)
    {
      return BadRequest(Result.Fail(nameof(id), "@errors.onupdate.noidmatch"));
    }

    Result<ZeroUser> result = await Users.Save(updateModel);

    if (Hints.ResponsePreference == ApiResponsePreference.Minimal)
    {
      return result.WithoutModel();
    }

    return result;
  }


  //[HttpDelete("{id}")]
  //[ZeroAuthorize(UserPermissions.Delete)]
  //public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}