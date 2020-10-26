using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace zero.Web.Controllers
{
  public class DefaultRouteController : Controller
  {
    public DefaultRouteController()
    {

    }

    public IActionResult Index()
    {
      return Json(HttpContext.Request.RouteValues, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.None });
    }
  }
}
