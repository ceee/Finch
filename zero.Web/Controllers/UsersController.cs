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


    public async Task<EditModel<IUser>> GetById([FromQuery] string id) => Edit(await Api.GetUserById(id));


    public async Task<ListResult<IUser>> GetAll([FromQuery] ListQuery<IUser> query) => await Api.GetByQuery(query);


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
    public async Task<EntityResult<IUser>> UpdatePassword([FromBody] UserPasswordEditModel model)
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

      return result;
    }


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<EntityResult<IUser>> Disable([FromBody] IUser model)
    {
      IUser entity = await Api.GetUserById(model.Id);
      return await Api.Disable(entity);
    }


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<EntityResult<IUser>> Enable([FromBody] IUser model)
    {
      IUser entity = await Api.GetUserById(model.Id);
      return await Api.Enable(entity);
    }


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<EntityResult<IUser>> Save([FromBody] IUser model) => await Api.Save(model);


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    // TODO do not need settings.users authorization for editing current user profiles
    public async Task<EntityResult<IUser>> SaveCurrent([FromBody] IUser model) => await Api.Save(model);


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<EntityResult<IUser>> Delete([FromQuery] string id) => await Api.Delete(id);
  }
}
