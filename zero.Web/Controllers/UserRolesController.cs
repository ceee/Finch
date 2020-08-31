using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;

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


    public async Task<IActionResult> GetById([FromQuery] string id) => Edit(await Api.GetById(id));


    public async Task<IActionResult> GetAll() => Json(await Api.GetAll());
   

    public IActionResult GetAllPermissions() => Json(PermissionsApi.GetAll());


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<IActionResult> Save([FromBody] IUserRole model) => Json(await Api.Save(model));


    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Update)]
    public async Task<IActionResult> Delete([FromQuery] string id) => Json(await Api.Delete(id));
  }
}
