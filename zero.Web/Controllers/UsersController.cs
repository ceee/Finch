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
    IUserRolesApi RolesApi;
    ILanguagesApi LanguagesApi;


    public UsersController(IUserApi api, IAuthenticationApi authenticationApi, IUserRolesApi rolesApi, ILanguagesApi languagesApi)
    {
      Api = api;
      AuthenticationApi = authenticationApi;
      RolesApi = rolesApi;
      LanguagesApi = languagesApi;
    }


    /// <summary>
    /// Get user by id
    /// </summary>    
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      User user = await Api.GetUserById(id);

      if (user == null)
      {
        return new StatusCodeResult(404);
      }

      UserEditModel model = await Mapper.Map<User, UserEditModel>(user);
      model.SupportedCultures = LanguagesApi.GetAllCultures(Options.SupportedLanguages);

      return Json(model);
    }


    /// <summary>
    /// Get all users
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<User> query)
    {
      return await As<User, UserListModel>(await Api.GetByQuery(query, "zero.applications.1-A"));
    }


    /// <summary>
    /// Get all permissions for selection
    /// </summary>    
    public IActionResult GetAllPermissions()
    {
      return Json(Options.Backoffice.Permissions);
    }


    /// <summary>
    /// Updates a user password
    /// </summary>
    [ZeroAuthorize]
    public async Task<IActionResult> UpdatePassword([FromBody] UserPasswordEditModel model)
    {
      EntityResult<User> result;

      if (model.NewPassword != model.ConfirmNewPassword)
      {
        result = EntityResult<User>.Fail(nameof(model.NewPassword), "@errors.changepassword.newpasswordsnotmatching");
      }
      else
      {
        User user = await AuthenticationApi.GetUser();
        result = await Api.UpdatePassword(user, model.CurrentPassword, model.NewPassword);

        if (result.IsSuccess)
        {
          await AuthenticationApi.Logout();
        }
      }

      return Json(result);
      //return await As<Translation, TranslationEditModel>(await Api.);
    }


    /// <summary>
    /// Disables a user
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Write)]
    public async Task<IActionResult> Disable([FromBody] UserEditModel model)
    {
      User entity = await Api.GetUserById(model.Id);
      return await As<User, UserEditModel>(await Api.Disable(entity));
    }


    /// <summary>
    /// Enables a user
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Write)]
    public async Task<IActionResult> Enable([FromBody] UserEditModel model)
    {
      User entity = await Api.GetUserById(model.Id);
      return await As<User, UserEditModel>(await Api.Enable(entity));
    }


    /// <summary>
    /// Save user
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Write)]
    public async Task<IActionResult> Save([FromBody] UserEditModel model)
    {
      User entity = await Mapper.Map(model, await Api.GetUserById(model.Id));
      return await As<User, UserEditModel>(await Api.Save(entity));
    }


    /// <summary>
    /// Update current user
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Write)]
    // TODO do not need settings.users authorization for editing current user profiles
    public async Task<IActionResult> SaveCurrent([FromBody] UserEditModel model)
    {
      User entity = await Mapper.Map(model, await Api.GetUserById(model.Id));
      return await As<User, UserEditModel>(await Api.Save(entity));
    }


    /// <summary>
    /// Deletes a user
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Write)]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return await As<User, UserEditModel>(await Api.Delete(id));
    }
  }
}
