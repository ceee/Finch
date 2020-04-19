using Microsoft.AspNetCore.Mvc;
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

    public UsersController(IZeroConfiguration config, IUserApi api, IMapper mapper) : base(config, mapper)
    {
      Api = api;
    }


    /// <summary>
    /// Get user by id
    /// </summary>    
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      return Json<User, UserEditModel>(await Api.GetUserById(id));
    }


    /// <summary>
    /// Get all users
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<User> query)
    {
      return Json(await Api.GetByQuery(query, "zero.applications.1-A"));
    }
  }
}
