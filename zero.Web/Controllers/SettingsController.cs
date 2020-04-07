using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zero.Core;
using zero.Core.Api;

namespace zero.Web.Controllers
{
  [AllowAnonymous]
  public class SettingsController : BackofficeController
  {
    private ISettingsApi Api { get; set; }

    public SettingsController(IZeroConfiguration config, ISettingsApi api) : base(config)
    {
      Api = api;
    }


    public IActionResult GetAreas()
    {
      return Json(Api.GetAreas());
    }
  }
}
