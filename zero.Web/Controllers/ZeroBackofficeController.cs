using Microsoft.AspNetCore.Mvc;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Core.Options;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(false)]
  public class ZeroBackofficeController : Controller
  {
    IZeroVue ZeroVue { get; set; }
    IZeroOptions Options { get; set; }

    public ZeroBackofficeController(IZeroVue zeroVue, IZeroOptions options)
    {
      ZeroVue = zeroVue;
      Options = options;
    }


    public IActionResult Index()
    {
      if (Options.ZeroVersion.IsNullOrEmpty())
      {
        return RedirectToAction("ZeroBackoffice", "Setup");
      }

      return View("Views/Zero/Index.cshtml", new ZeroBackofficeModel()
      {
        Vue = ZeroVue
      });
    }
  }
}
