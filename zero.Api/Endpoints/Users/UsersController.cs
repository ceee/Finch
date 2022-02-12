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

    model.PasswordHash = null;
    model.SecurityStamp = null;

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


  [HttpPost("")]
  [ZeroAuthorize(UserPermissions.Create)]
  public virtual async Task<ActionResult<Result>> Create(ZeroUser saveModel)
  {
    Result<ZeroUser> result = await Users.Save(saveModel);

    bool minimalResponse = Hints.ResponsePreference == ApiResponsePreference.Minimal;

    if (result.IsSuccess)
    {
      return Created("/", minimalResponse ? null : saveModel);
    }

    return result.WithoutModel();
  }


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


  [HttpDelete("{id}")]
  [ZeroAuthorize(UserPermissions.Delete)]
  public virtual async Task<ActionResult<Result>> Delete(string id)
  {
    Result<ZeroUser> result = await Users.Delete(id);
    return result.WithoutModel();
  }


  [HttpGet("password/random/{length?}")]
  [ZeroAuthorize(UserPermissions.Read)]
  public virtual ActionResult<dynamic> Password(int length = -1)
  {
    return new
    {
      Password = PasswordGenerator.Random(length)
    };
  }
}