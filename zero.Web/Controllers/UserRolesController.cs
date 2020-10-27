using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Read)]
  public class UserRolesController : BackofficeController
  {
    IUserRolesApi Api;
    IPermissionsApi PermissionsApi;


    public UserRolesController(IUserRolesApi api, IPermissionsApi permissionsApi)
    {
      Api = api;
      PermissionsApi = permissionsApi;
    }


    public async Task<EditModel<IUserRole>> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public async Task<IList<IUserRole>> GetAll() => await Api.GetAll();
   

    public IList<PermissionCollection> GetAllPermissions() => PermissionsApi.GetAll();


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<EntityResult<IUserRole>> Save([FromBody] IUserRole model) => await Api.Save(model);


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<EntityResult<IUserRole>> Delete([FromQuery] string id) => await Api.Delete(id);
  }
}
