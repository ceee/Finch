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
    IUserRolesService Api;
    IPermissionsService PermissionsApi;


    public UserRolesController(IUserRolesService api, IPermissionsService permissionsApi)
    {
      Api = api;
      PermissionsApi = permissionsApi;
    }


    public async Task<EditModel<ZeroUserRole>> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public async Task<IList<ZeroUserRole>> GetAll() => await Api.GetAll();
   

    public IList<PermissionCollection> GetAllPermissions() => PermissionsApi.GetAll();


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<EntityResult<ZeroUserRole>> Save([FromBody] ZeroUserRole model) => await Api.Save(model);


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<EntityResult<ZeroUserRole>> Delete([FromQuery] string id) => await Api.Delete(id);
  }
}
