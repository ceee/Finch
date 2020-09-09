using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Read)]
  public class UsersController : BackofficeController 
  {
    IUserApi Api;
    IAuthenticationApi AuthenticationApi;
    IPermissionsApi PermissionsApi;


    public UsersController(IUserApi api, IAuthenticationApi authenticationApi, IPermissionsApi permissionsApi)
    {
      Api = api;
      AuthenticationApi = authenticationApi;
      PermissionsApi = permissionsApi;
    }


    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetUserById(id));


    public async Task<IActionResult> GetAll([FromQuery] ListQuery<IUser> query) => Json(await Api.GetByQuery(query));


    public IActionResult GetAllPermissions() => Json(PermissionsApi.GetAll());


    [ZeroAuthorize]
    public async Task<IActionResult> UpdatePassword([FromBody] UserPasswordEditModel model)
    {
      EntityResult<IUser> result;

      if (model.NewPassword != model.ConfirmNewPassword)
      {
        result = EntityResult<IUser>.Fail(nameof(model.NewPassword), "@errors.changepassword.newpasswordsnotmatching");
      }
      else
      {
        User user = await AuthenticationApi.GetUser();
        result = await Api.UpdatePassword(user as IUser, model.CurrentPassword, model.NewPassword);

        if (result.IsSuccess)
        {
          await AuthenticationApi.Logout();
        }
      }

      return Json(result);
    }


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<IActionResult> Disable([FromBody] IUser model)
    {
      IUser entity = await Api.GetUserById(model.Id);
      return Json(await Api.Disable(entity));
    }


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<IActionResult> Enable([FromBody] IUser model)
    {
      IUser entity = await Api.GetUserById(model.Id);
      return Json(await Api.Enable(entity));
    }


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<IActionResult> Save([FromBody] IUser model) => Json(await Api.Save(model));


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    // TODO do not need settings.users authorization for editing current user profiles
    public async Task<IActionResult> SaveCurrent([FromBody] IUser model) => Json(await Api.Save(model));


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
