using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;
using zero.Core.Routing;

namespace zero.Web.Controllers
{
  public class ZeroFrontendController : ZeroController<PageRoute>
  {
    public ZeroFrontendController()
    {
    }

    public IActionResult Index()
    {
      return Json(new { Application, Route }, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.None });
    }
  }
}
