using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Filters;
using zero.Web.Mapper;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Read)]
  public class UserRolesController : BackofficeController
  {
    private IUserRolesApi Api { get; set; }

    private ZeroOptions Options { get; set; }


    public UserRolesController(IZeroConfiguration config, IUserRolesApi api, IMapper mapper, IToken token, IOptionsMonitor<ZeroOptions> options) : base(config, mapper, token)
    {
      Api = api;
      Options = options.CurrentValue;
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
      return Json(Options.Authorization.Permissions);
    }


    /// <summary>
    /// Save country
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Write)]
    public async Task<IActionResult> Save([FromBody] UserRoleEditModel model)
    {
      var state = ModelState;
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
