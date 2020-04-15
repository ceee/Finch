using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using zero.Core;
using zero.Core.Auth;
using zero.Core.Extensions;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(false)]
  public class IndexController : BackofficeController
  {
    private ZeroOptions Options { get; set; }

    public IndexController(IZeroConfiguration config, IOptionsMonitor<ZeroOptions> options) : base(config)
    {
      Options = options.CurrentValue;
    }


    public IActionResult Index()
    {
      if (Configuration.ZeroVersion.IsNullOrEmpty())
      {
        return RedirectToAction("Index", "Setup");
      }

      return View("/Views/Index.cshtml", Options);
    }
  }
}
