using Microsoft.AspNetCore.Mvc;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(false)]
  public class IndexController : BackofficeController
  {
    private IZeroVue ZeroVue { get; set; }

    public IndexController(IZeroVue zeroVue)
    {
      ZeroVue = zeroVue;
    }


    public IActionResult Index()
    {
      if (Options.ZeroVersion.IsNullOrEmpty())
      {
        return RedirectToAction("Index", "Setup");
      }

      return View(new ZeroBackofficeModel()
      {
        Vue = ZeroVue
      });
    }
  }
}
