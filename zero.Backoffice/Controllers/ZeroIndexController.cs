using Microsoft.AspNetCore.Mvc;

namespace zero.Backoffice.Controllers;

[ZeroAuthorize(false)]
[DisableBrowserCache]
public class ZeroIndexController : Controller
{
  IZeroVue ZeroVue { get; set; }
  IZeroOptions Options { get; set; }

  public ZeroIndexController(IZeroVue zeroVue, IZeroOptions options)
  {
    ZeroVue = zeroVue;
    Options = options;
  }


  public IActionResult Index()
  {
    if (Options.Version.IsNullOrEmpty())
    {
      return RedirectToAction("ZeroBackoffice", "Setup");
    }

    return View("Views/Zero/Index.cshtml", new ZeroBackofficeModel()
    {
      Port = Options.For<BackofficeOptions>().DevServer.Port,
      Vue = ZeroVue
    });
  }
}

public class ZeroBackofficeModel
{
  public int Port { get; set; }

  public IZeroVue Vue { get; set; }
}
