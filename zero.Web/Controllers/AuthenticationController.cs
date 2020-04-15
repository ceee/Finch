using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Auth;

namespace zero.Web.Controllers
{
  [ZeroAuthorize]
  public class AuthenticationController : BackofficeController
  {
    private IAuthenticationApi Api { get; set; }

    public AuthenticationController(IZeroConfiguration config, IAuthenticationApi api) : base(config)
    {
      Api = api;
    }


    /// <summary>
    /// Get the currently logged in user
    /// </summary>
    public async Task<IActionResult> GetUser()
    {
      return Json(await Api.GetUser());
    }


    /// <summary>
    /// Tries a login for a user with username/password
    /// </summary>
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> LoginUser()
    {
      await Task.Delay(0);
      throw new NotImplementedException();
      //User user = await Api.Login(model.Username, model.Password, model.RememberMe);

      //if (user == null)
      //{
      //  return AsError("Username or password is incorrect");
      //}

      //await Api.SetLastSeen(user);

      //return Json(new
      //{
      //  IsSuccess = true,
      //  User = new Results.User(user),
      //  UserMap = await Api.GetUserMap()
      //});
    }


    /// <summary>
    /// Logout for the current user
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> LogoutUser()
    {
      await Task.Delay(0);
      throw new NotImplementedException();
      //await Api.Logout();
      //return AsSuccess();
    }
  }
}
