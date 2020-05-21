using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(false)]
  public class AuthenticationController : BackofficeController
  {
    IAuthenticationApi Api;
    IApplicationContext AppContext;


    public AuthenticationController(IAuthenticationApi api, IApplicationContext appContext)
    {
      Api = api;
      AppContext = appContext;
    }


    /// <summary>
    /// Returns the current user
    /// </summary>
    public async Task<IActionResult> GetUser()
    {
      return await As<User, UserEditModel>(await Api.GetUser());
    }


    /// <summary>
    /// If a user is logged in
    /// </summary>    
    public IActionResult IsLoggedIn()
    {
      return Json(EntityResult.Maybe(Api.IsLoggedIn()));
    }


    /// <summary>
    /// Tries a login for a user with username/password
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> LoginUser([FromBody] LoginModel model)
    {
      EntityResult result = await Api.Login(model.Email, model.Password, model.IsPersistent);
      return Json(result);
    }


    /// <summary>
    /// Logout for the current user
    /// </summary>
    [HttpPost, ZeroAuthorize]
    public async Task<IActionResult> LogoutUser()
    {
      await Api.Logout();
      return Json(EntityResult.Success());
    }


    /// <summary>
    /// Try to switch selected application for user
    /// </summary>
    [HttpPost, ZeroAuthorize]
    public async Task<IActionResult> SwitchApp(string appId)
    {
      User user = await Api.GetUser();
      bool isSuccess = await AppContext.TrySwitchForUser(user, appId);

      return Json(new EntityResult() 
      { 
        IsSuccess = isSuccess 
      });
    }
  }
}
