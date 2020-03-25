using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zero.Core;
using zero.Core.Extensions;

namespace zero.Web.Controllers
{
  [AllowAnonymous]
  public class IndexController : BackofficeController
  {
    public IndexController(IZeroConfiguration config) : base(config)
    {

    }


    public IActionResult Index()
    {
      if (Configuration.ZeroVersion.IsNullOrEmpty())
      {
        return RedirectToAction("Index", "Setup");
      }

      return View("/Views/Index.cshtml");
    }
  }
}
