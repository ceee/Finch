using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Web.Controllers
{
  public abstract class ZeroController : ZeroController<IResolvedRoute>
  {
    
  }


  public abstract class ZeroController<T> : Controller where T : IResolvedRoute
  {
    protected IApplication Application { get; set; }

    protected virtual T Route { get; set; }


    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      Application = filterContext.HttpContext.RequestServices.GetService<IApplicationContext>()?.App;

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
