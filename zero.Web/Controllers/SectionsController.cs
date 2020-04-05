using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using zero.Core;

namespace zero.Web.Controllers
{
  [AllowAnonymous]
  public class SectionsController : BackofficeController
  {
    private ZeroOptions Options { get; set; }

    public SectionsController(IZeroConfiguration config, IOptionsMonitor<ZeroOptions> options) : base(config)
    {
      Options = options.CurrentValue;
    }


    public IActionResult GetAll()
    {
      return Json(Options.Sections.ToList());
    }
  }
}
