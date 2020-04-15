using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using zero.Core;
using zero.Core.Auth;
using zero.Core.Extensions;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(false)]
  public class IndexController : BackofficeController
  {
    private ZeroOptions Options { get; set; }

    private IZeroVue ZeroVue { get; set; }

    public IndexController(IZeroConfiguration config, IZeroVue zeroVue) : base(config)
    {
      ZeroVue = zeroVue;
    }


    public IActionResult Index()
    {
      if (Configuration.ZeroVersion.IsNullOrEmpty())
      {
        return RedirectToAction("Index", "Setup");
      }

      return View("/Views/Index.cshtml", new ZeroBackofficeModel()
      {
        Vue = ZeroVue
      });
    }
  }
}
