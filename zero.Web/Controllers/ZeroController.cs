using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Routing;

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
      if (filterContext.RouteData.Values.TryGetValue("zero.app", out object app))
      {
        Application = (IApplication)app;
      }

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
