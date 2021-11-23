using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using zero.Core.Api;
using zero.Setup;

namespace zero.Backoffice.Endpoints;

[ZeroAuthorize(false)]
public class ZeroSetupController : Controller
{
  ISetupApi Api;
  IWebHostEnvironment Env;
  IZeroOptions Options;


  public ZeroSetupController(ISetupApi api, IWebHostEnvironment env, IZeroOptions options)
  {
    Api = api;
    Env = env;
    Options = options;
  }


  public IActionResult Index()
  {
    //if (!Options.ZeroVersion.IsNullOrEmpty())
    //{
    //  return Redirect(Options.BackofficePath);
    //}

    return View("/Views/Zero/Setup.cshtml");
  }


  [HttpPost]
  public async Task<IActionResult> Install([FromBody] SetupModel model)
  {
    model.ContentRootPath = Env.ContentRootPath;

    EntityResult<SetupModel> result = await Api.Install(model);

    if (result.IsSuccess)
    {
      return Json(result);
    }

    object value = String.Join("\n\n", result.Errors.Select(error => error.Message + "\n(property: " + error.Property + ")"));
    return StatusCode(500, value);
  }
}