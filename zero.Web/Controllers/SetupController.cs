using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Entities.Setup;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Web.Controllers;

namespace zero.Web.Setup
{
  [ZeroAuthorize(false)]
  public class SetupController : BackofficeController
  {
    ISetupApi Api;
    IWebHostEnvironment Env;


    public SetupController(ISetupApi api, IWebHostEnvironment env)
    {
      Api = api;
      Env = env;
    }


    public IActionResult Index()
    {
      if (!Options.ZeroVersion.IsNullOrEmpty())
      {
        return Redirect(Options.BackofficePath);
      }

      return View("/Views/Setup.cshtml");
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
}
