using Microsoft.AspNetCore.Mvc;
using zero.Core;
using zero.Core.Api;
using zero.Core.Auth;

namespace zero.Web.Controllers
{
  [ZeroAuthorize("tobi")]
  public class TestController : BackofficeController
  {
    private IAuthenticationApi Api { get; set; }

    public TestController(IZeroConfiguration config, IAuthenticationApi api) : base(config)
    {
      Api = api;
    }

    public IActionResult GetUser()
    {
      return Json(new
      {
        action = "GetUser"
      });
    }
  }
}
