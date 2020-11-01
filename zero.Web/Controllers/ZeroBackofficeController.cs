using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Core.Options;
using zero.Web.Models;
using Zero.Web.DevServer;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(false)]
  public class ZeroBackofficeController : Controller
  {
    IZeroVue ZeroVue { get; set; }
    IZeroOptions Options { get; set; }
    IOptions<ZeroDevOptions> DevServerOptions { get; set; }

    public ZeroBackofficeController(IZeroVue zeroVue, IZeroOptions options, IOptions<ZeroDevOptions> devServerOptions)
    {
      ZeroVue = zeroVue;
      Options = options;
      DevServerOptions = devServerOptions;
    }


    public IActionResult Index()
    {
      if (Options.ZeroVersion.IsNullOrEmpty())
      {
        return RedirectToAction("ZeroBackoffice", "Setup");
      }

      return View("Views/Zero/Index.cshtml", new ZeroBackofficeModel()
      {
        Port = DevServerOptions.Value.Port,
        Vue = ZeroVue
      });
    }
  }
}
