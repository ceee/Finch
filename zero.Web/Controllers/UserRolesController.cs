using Microsoft.AspNetCore.Mvc;
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


    /// <summary>
    /// Get role by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      return await As<UserRole, UserRoleEditModel>(await Api.GetById(id));
    }


    /// <summary>
    /// Get all roles
    /// </summary>    
    public async Task<IActionResult> GetAll()
    {
      return await As<UserRole, UserRoleListModel>(await Api.GetAll());
    }


    /// <summary>
    /// Get all permissions for selection
    /// </summary>    
    public IActionResult GetAllPermissions()
    {
      return Json(PermissionsApi.GetAll());
    }


    /// <summary>
    /// Save country
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Write)]
    public async Task<IActionResult> Save([FromBody] UserRoleEditModel model)
    {
      UserRole role = await Mapper.Map(model, await Api.GetById(model.Id));
      return await As<UserRole, UserRoleEditModel>(await Api.Save(role));
    }


    /// <summary>
    /// Deletes a country
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Write)]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return await As<UserRole, UserRoleEditModel>(await Api.Delete(id));
    }
  }
}
