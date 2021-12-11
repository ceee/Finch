//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using zero.Core.Api;
//using zero.Core.Entities;
//using zero.Core.Identity;
//using zero.Web.Models;

//namespace zero.Web.Controllers
//{
//  [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Read)]
//  public class UsersController : BackofficeController 
//  {
//    IUserService Api;
//    IAuthenticationService AuthenticationApi;
//    IPermissionsService PermissionsApi;


//    public UsersController(IUserService api, IAuthenticationService authenticationApi, IPermissionsService permissionsApi)
//    {
//      Api = api;
//      AuthenticationApi = authenticationApi;
//      PermissionsApi = permissionsApi;
//      IsCoreDatabase = true;
//    }


//    public async Task<EditModel<ZeroUser>> GetById([FromQuery] string id) => Edit(await Api.GetUserById(id));


//    public async Task<Paged<ZeroUser>> GetAll([FromQuery] ListQuery<ZeroUser> query) => await Api.GetByQuery(query);


//    public IList<PermissionCollection> GetAllPermissions() => PermissionsApi.GetAll();


//    public async Task<IEnumerable<PickerModel>> GetForPicker() => (await Api.GetAll()).Select(x => new PickerModel()
//    {
//      Id = x.Id,
//      Name = x.Name,
//      IsActive = x.IsActive
//    });


//    public async Task<IList<PickerPreviewModel>> GetPreviews([FromQuery] List<string> ids)
//    {
//      return Previews(await Api.GetByIds(ids.ToArray()), item => new PickerPreviewModel()
//      {
//        Id = item.Id,
//        Icon = item.AvatarId,
//        Name = item.Name
//      });
//    }


//    [ZeroAuthorize]
//    public async Task<Result<ZeroUser>> UpdatePassword([FromBody] UserPasswordEditModel model)
//    {
//      Result<ZeroUser> result;

//      if (model.NewPassword != model.ConfirmNewPassword)
//      {
//        result = Result<ZeroUser>.Fail(nameof(model.NewPassword), "@errors.changepassword.newpasswordsnotmatching");
//      }
//      else
//      {
//        ZeroUser user = await AuthenticationApi.GetUser();
//        result = await Api.UpdatePassword(user as ZeroUser, model.CurrentPassword, model.NewPassword);

//        if (result.IsSuccess)
//        {
//          await AuthenticationApi.Logout();
//        }
//      }

//      return result;
//    }


//    [ZeroAuthorize]
//    public async Task<Result<string>> HashPassword([FromBody] UserPasswordEditModel model)
//    {
//      ZeroUser user = await Api.GetUserById(model.UserId);
//      return await Api.HashPassword(user, model.CurrentPassword, model.NewPassword, model.ConfirmNewPassword);
//    }


//    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
//    public async Task<Result<ZeroUser>> Disable([FromBody] ZeroUser model)
//    {
//      ZeroUser entity = await Api.GetUserById(model.Id);
//      return await Api.Disable(entity);
//    }


//    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
//    public async Task<Result<ZeroUser>> Enable([FromBody] ZeroUser model)
//    {
//      ZeroUser entity = await Api.GetUserById(model.Id);
//      return await Api.Enable(entity);
//    }


//    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
//    public async Task<Result<ZeroUser>> Save([FromBody] ZeroUser model) => await Api.Save(model);


//    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
//    // TODO do not need settings.users authorization for editing current user profiles
//    public async Task<Result<ZeroUser>> SaveCurrent([FromBody] ZeroUser model) => await Api.Save(model);


//    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
//    public async Task<Result<ZeroUser>> Delete([FromQuery] string id) => await Api.Delete(id);
//  }
//}
