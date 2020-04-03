using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Entities.Setup;
using zero.Web.Controllers;

namespace zero.Web.Setup
{
  [AllowAnonymous]
  public class SetupController : BackofficeController
  {
    protected ISetupApi Api { get; private set; }


    public SetupController(IZeroConfiguration config, ISetupApi api) : base(config)
    {
      Api = api;
    }

    public IActionResult Index()
    {
      return View("/Views/Setup.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Install([FromBody] SetupModel model)
    {
      EntityChangeResult<SetupModel> result = await Api.Install(model);
      return Json(result);
    }
  }
}
