using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;
using zero.Core.Routing;

namespace zero.Web.Controllers
{
  public class DefaultRouteController : ZeroController<PageRoute>
  {
    public DefaultRouteController()
    {
    }

    public IActionResult Index()
    {
      return Json(new { Application, Route }, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.None });
    }
  }
}
