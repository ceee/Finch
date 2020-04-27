using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Mapper;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Users, PermissionsValue.Read)]
  public class UsersController : BackofficeController
  {
    private IUserApi Api { get; set; }

    private ZeroOptions Options { get; set; }

    public UsersController(IZeroConfiguration config, IUserApi api, IMapper mapper, IToken token, IOptionsMonitor<ZeroOptions> options) : base(config, mapper, token)
    {
      Api = api;
      Options = options.CurrentValue;
    }


    /// <summary>
    /// Get user by id
    /// </summary>    
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      return As<User, UserEditModel>(await Api.GetUserById(id));
    }


    /// <summary>
    /// Get all users
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<User> query)
    {
      return As<User, UserListModel>(await Api.GetByQuery(query, "zero.applications.1-A"));
    }


    /// <summary>
    /// Get all permissions for selection
    /// </summary>    
    public IActionResult GetAllPermissions()
    {
      return Json(Options.Authorization.Permissions);
    }
  }
}
