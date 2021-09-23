using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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


    public async Task<EditModel<BackofficeUser>> GetById([FromQuery] string id) => Edit(await Api.GetUserById(id));


    public async Task<ListResult<BackofficeUser>> GetAll([FromQuery] ListBackofficeQuery<BackofficeUser> query) => await Api.GetByQuery(query);


    public IList<PermissionCollection> GetAllPermissions() => PermissionsApi.GetAll();


    public async Task<IEnumerable<SelectModel>> GetForPicker() => (await Api.GetAll()).Select(x => new SelectModel()
    {
      Id = x.Id,
      Name = x.Name,
      IsActive = x.IsActive
    });


    public async Task<IList<PreviewModel>> GetPreviews([FromQuery] List<string> ids)
    {
      return Previews(await Api.GetByIds(ids.ToArray()), item => new PreviewModel()
      {
        Id = item.Id,
        Icon = item.AvatarId,
        Name = item.Name
      });
    }


    [ZeroAuthorize]
    public async Task<EntityResult<BackofficeUser>> UpdatePassword([FromBody] UserPasswordEditModel model)
    {
      EntityResult<BackofficeUser> result;

      if (model.NewPassword != model.ConfirmNewPassword)
      {
        result = EntityResult<BackofficeUser>.Fail(nameof(model.NewPassword), "@errors.changepassword.newpasswordsnotmatching");
      }
      else
      {
        BackofficeUser user = await AuthenticationApi.GetUser();
        result = await Api.UpdatePassword(user as BackofficeUser, model.CurrentPassword, model.NewPassword);

        if (result.IsSuccess)
        {
          await AuthenticationApi.Logout();
        }
      }

      return result;
    }


    [ZeroAuthorize]
    public async Task<EntityResult<string>> HashPassword([FromBody] UserPasswordEditModel model)
    {
      BackofficeUser user = await Api.GetUserById(model.UserId);
      return await Api.HashPassword(user, model.CurrentPassword, model.NewPassword, model.ConfirmNewPassword);
    }


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<EntityResult<BackofficeUser>> Disable([FromBody] BackofficeUser model)
    {
      BackofficeUser entity = await Api.GetUserById(model.Id);
      return await Api.Disable(entity);
    }


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<EntityResult<BackofficeUser>> Enable([FromBody] BackofficeUser model)
    {
      BackofficeUser entity = await Api.GetUserById(model.Id);
      return await Api.Enable(entity);
    }


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<EntityResult<BackofficeUser>> Save([FromBody] BackofficeUser model) => await Api.Save(model);


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    // TODO do not need settings.users authorization for editing current user profiles
    public async Task<EntityResult<BackofficeUser>> SaveCurrent([FromBody] BackofficeUser model) => await Api.Save(model);


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<EntityResult<BackofficeUser>> Delete([FromQuery] string id) => await Api.Delete(id);
  }
}
