using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Read)]
  public class UsersController<T, TLanguage> : BackofficeController 
    where T : class, IUser
    where TLanguage : ILanguage
  {
    IUserApi<T> Api;
    IAuthenticationApi AuthenticationApi;
    IUserRolesApi RolesApi;
    ILanguagesApi<TLanguage> LanguagesApi;
    IPermissionsApi PermissionsApi;


    public UsersController(IUserApi<T> api, IAuthenticationApi authenticationApi, IUserRolesApi rolesApi, ILanguagesApi<TLanguage> languagesApi, IPermissionsApi permissionsApi)
    {
      Api = api;
      AuthenticationApi = authenticationApi;
      RolesApi = rolesApi;
      LanguagesApi = languagesApi;
      PermissionsApi = permissionsApi;
    }


    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetUserById(id));


    public IActionResult GetSupportedCultures() => Json(LanguagesApi.GetAllCultures(Options.SupportedLanguages));


    public async Task<IActionResult> GetAll([FromQuery] ListQuery<T> query) => Json(await Api.GetByQuery(query));


    public IActionResult GetAllPermissions() => Json(PermissionsApi.GetAll());


    [ZeroAuthorize]
    public async Task<IActionResult> UpdatePassword([FromBody] UserPasswordEditModel model)
    {
      EntityResult<T> result;

      if (model.NewPassword != model.ConfirmNewPassword)
      {
        result = EntityResult<T>.Fail(nameof(model.NewPassword), "@errors.changepassword.newpasswordsnotmatching");
      }
      else
      {
        User user = await AuthenticationApi.GetUser();
        result = await Api.UpdatePassword(user as T, model.CurrentPassword, model.NewPassword);

        if (result.IsSuccess)
        {
          await AuthenticationApi.Logout();
        }
      }

      return Json(result);
    }


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<IActionResult> Disable([FromBody] T model)
    {
      T entity = await Api.GetUserById(model.Id);
      return Json(await Api.Disable(entity));
    }


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<IActionResult> Enable([FromBody] T model)
    {
      T entity = await Api.GetUserById(model.Id);
      return Json(await Api.Enable(entity));
    }


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<IActionResult> Save([FromBody] T model) => Json(await Api.Save(model));


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    // TODO do not need settings.users authorization for editing current user profiles
    public async Task<IActionResult> SaveCurrent([FromBody] T model) => Json(await Api.Save(model));


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
