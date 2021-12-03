using Microsoft.AspNetCore.Mvc;
using zero.Api.Filters;
using zero.Identity.Models;

namespace zero.Backoffice.Endpoints.Account;

[ZeroSystemApi]
public class AccountController : ZeroBackofficeController
{
  readonly IAuthenticationService AuthService;

  public AccountController(IAuthenticationService authService)
  {
    AuthService = authService;
  }


  [HttpGet("user")]
  public async Task<ActionResult<UserModel>> GetUser()
  {
    ZeroUser user = await AuthService.GetUser();

    if (user == null)
    {
      return BadRequest("notfound");
    }

    return Mapper.Map<ZeroUser, UserModel>(user);
  }


  [HttpGet("loggedin")]
  [ZeroAuthorize(false)]
  public ActionResult<bool> LoggedIn() => AuthService.IsLoggedIn();


  [HttpPost("login")]
  [ZeroAuthorize(false)]
  //[ValidateAntiForgeryToken]
  public async Task<ActionResult> Login(LoginModel model)
  {
    LoginResult result = await AuthService.Login(model.Email, model.Password, model.IsPersistent);

    if (result != LoginResult.Success)
    {
      return BadRequest(result.ToString()); // TODO translate resource
    }

    return Ok();
  }


  [HttpPost("logout")]
  public async Task<ActionResult> Logout()
  {
    await AuthService.Logout();
    return Ok();
  }


  //[HttpPost, ZeroAuthorize]
  //public async Task<Result> SwitchApp(string appId)
  //{
  //  return Result.Maybe(await Api.TrySwitchApp(appId));
  //}
}