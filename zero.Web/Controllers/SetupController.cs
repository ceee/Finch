using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Auth;
using zero.Core.Entities;
using zero.Core.Entities.Setup;
using zero.Core.Extensions;
using zero.Web.Controllers;

namespace zero.Web.Setup
{
  [ZeroAuthorize(false)]
  public class SetupController : BackofficeController
  {
    protected ISetupApi Api { get; private set; }

    protected IWebHostEnvironment Environment { get; private set; }

    protected ZeroOptions Options { get; private set; }


    public SetupController(IZeroConfiguration config, ISetupApi api, IWebHostEnvironment env, IOptionsMonitor<ZeroOptions> options) : base(config) //  UserManager<User> userManager
    {
      Api = api;
      Environment = env;
      Options = options.CurrentValue;
    }


    public IActionResult Index()
    {
      if (!Configuration.ZeroVersion.IsNullOrEmpty())
      {
        return Redirect(Options.BackofficePath);
      }

      return View("/Views/Setup.cshtml");
    }


    [HttpPost]
    public async Task<IActionResult> Install([FromBody] SetupModel model)
    {
      model.ContentRootPath = Environment.ContentRootPath;

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
