using Microsoft.AspNetCore.Components;
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


    [HttpGet]
    public IActionResult GetUser()
    {
      return Json(new
      {
        user = HttpContext.User.Identity.IsAuthenticated
      });
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
  }
}
