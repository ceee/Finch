using Microsoft.AspNetCore.Mvc;
using zero.Routing;

namespace zero.Web;

public class ZeroFallbackController : ZeroController<PageRoute>
{
  public IActionResult Index()
  {
    return Json(new { Application, Route });
  }
}