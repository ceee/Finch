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
    IAuthenticationService Api;


    public AuthenticationController(IAuthenticationService api)
    {
      Api = api;
    }


    public async Task<EditModel<ZeroUser>> GetUser() => Edit(await Api.GetUser());

 
    public Result IsLoggedIn() => Result.Maybe(Api.IsLoggedIn());


    [HttpPost]
    public async Task<Result> LoginUser([FromBody] LoginModel model) => await Api.Login(model.Email, model.Password, model.IsPersistent);


    [HttpPost, ZeroAuthorize]
    public async Task<Result> LogoutUser()
    {
      await Api.Logout();
      return Result.Success();
    }


    [HttpPost, ZeroAuthorize]
    public async Task<Result> SwitchApp(string appId)
    {
      return Result.Maybe(await Api.TrySwitchApp(appId));
    }
  }
}
