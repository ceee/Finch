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

 
    public EntityResult IsLoggedIn() => EntityResult.Maybe(Api.IsLoggedIn());


    [HttpPost]
    public async Task<EntityResult> LoginUser([FromBody] LoginModel model) => await Api.Login(model.Email, model.Password, model.IsPersistent);


    [HttpPost, ZeroAuthorize]
    public async Task<EntityResult> LogoutUser()
    {
      await Api.Logout();
      return EntityResult.Success();
    }


    [HttpPost, ZeroAuthorize]
    public async Task<EntityResult> SwitchApp(string appId)
    {
      return EntityResult.Maybe(await Api.TrySwitchApp(appId));
    }
  }
}
