using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zero.Core;
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
      return View("/Setup/Setup.cshtml");
    }
  }
}
