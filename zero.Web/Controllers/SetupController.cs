using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Entities.Setup;
using zero.Web.Controllers;

namespace zero.Web.Setup
{
  [AllowAnonymous]
  public class SetupController : BackofficeController
  {
    public SetupController(IZeroConfiguration config) : base(config)
    {

    }

    public IActionResult Index()
    {
      return View("/Views/Setup.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Save(SetupSave model)
    {
      return Ok();
    }
  }
}
