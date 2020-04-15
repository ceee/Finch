using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Auth;
using zero.Core.Entities;

namespace zero.Web.Controllers
{
  //[ZeroAuthorize("tobi")]
  public class TestController : BackofficeController
  {
    private IAuthenticationApi Api { get; set; }

    private SignInManager<User> SignInManager;

    public TestController(IZeroConfiguration config, IAuthenticationApi api, SignInManager<User> signInManager) : base(config)
    {
      Api = api;
      SignInManager = signInManager;
    }


    [HttpPost]
    public async Task<IActionResult> Login([FromQuery] string username, [FromQuery] string password)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await SignInManager.PasswordSignInAsync(username, password, false, true);

      return Json(new 
      {
        username,
        password,
        result
      });
    }


    [ZeroAuthorize]
    public async Task<IActionResult> GetUser()
    {
      User user = await SignInManager.UserManager.GetUserAsync(HttpContext.User);

      return Json(new
      {
        user
      });
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
      await SignInManager.SignOutAsync();

      return Json(new
      {
        success = true
      });
    }
  }
}
