using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Core.Renderer;

namespace zero.Web.Controllers
{
  //[ZeroAuthorize("tobi")]
  public class TestController : BackofficeController
  {
    IAuthenticationApi Api;
    ISpacesApi SpacesApi;
    ILanguagesApi LanguagesApi;
    SignInManager<User> SignInManager;


    public TestController(IAuthenticationApi api, ISpacesApi spacesApi, ILanguagesApi languagesApi, SignInManager<User> signInManager)
    {
      Api = api;
      SpacesApi = spacesApi;
      LanguagesApi = languagesApi;
      SignInManager = signInManager;
    }


    [HttpGet]
    [ZeroAuthorize(false)]
    public IActionResult GetPlugins([FromServices] IEnumerable<IZeroPlugin> plugins)
    {
      return Json(plugins);
    }


    //[HttpGet]
    //[ZeroAuthorize(false)]
    //public IActionResult RenderConfig()
    //{
    //  SocialContentRenderer renderer = new SocialContentRenderer();

    //  return Json(renderer.Build());
    //}


    //[HttpGet]
    //[ZeroAuthorize(false)]
    //public async Task<IActionResult> SaveSpaceContent()
    //{
    //  TeamMember model = new TeamMember()
    //  {
    //    IsActive = true,
    //    Email = "tobi@test.com",
    //    Name = "Tobi",
    //    Position = "Chef",
    //    VideoUri = "https://swcs.pro"
    //  };

    //  model.Addresses.Add(new TeamMemberAddress()
    //  {
    //    City = "Braunau",
    //    Street = "My street",
    //    No = "23"
    //  });

    //  return Json(await SpacesApi.Save("team", model));
    //}


    [HttpGet]
    [ZeroAuthorize(false)]
    public async Task<IActionResult> AddLanguage()
    {
      return Json(await LanguagesApi.Save(new Language()
      {

        IsActive = true,
        Name = "English",
        Code = "en-US",
        IsDefault = true
      }));
    }


    //[HttpGet]
    //[ZeroAuthorize(false)]
    //public async Task<IActionResult> GetSpaceList()
    //{
    //  return Json(await SpacesApi.GetList<TeamMember>("team"));
    //}


    [HttpGet]
    [ZeroAuthorize(false)]
    public IActionResult GetRenderer([FromQuery] string alias)
    {
      Space space = SpacesApi.GetByAlias(alias);

      AbstractGenericRenderer renderer = Options.Renderers.GetAllItems().FirstOrDefault(x => x.TargetType == space.Type);

      if (renderer == null)
      {
        return NotFound();
      }

      return Json(renderer.Build());
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


    [ZeroAuthorize]
    public IActionResult GetUserClaims()
    {
      return Json(HttpContext.User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray());
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
