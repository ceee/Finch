using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Routing;
using Microsoft.Extensions.DependencyInjection;
using zero.Core;

namespace zero.Web.Controllers
{
  public abstract class ZeroController : ZeroController<IResolvedRoute>
  {
    
  }


  public abstract class ZeroController<T> : Controller where T : IResolvedRoute
  {
    public Application Application { get; set; }

    public virtual T Route { get; set; }

    IZeroContext _context;
    public IZeroContext Context => _context ?? (_context = HttpContext?.RequestServices?.GetService<IZeroContext>());


    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      Application = Context?.Application;

      if (filterContext.RouteData.Values.TryGetValue("zero.route", out object route))
      {
        if (!(route is T))
        {
          throw new InvalidCastException($"Could not cast IResolvedRoute to {typeof(T)}");
        }
        Route = (T)route;
      }
      base.OnActionExecuting(filterContext);
    }
  }
}
