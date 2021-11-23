using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using zero.Applications;
using zero.Context;
using zero.Routing;

namespace zero.Web;

public abstract class ZeroController : ZeroController<RouteModel> { }


public abstract class ZeroController<T> : Controller where T : class, IRouteModel
{
  public virtual Application Application => Context?.Application;

  T _route;
  public virtual T Route => _route ?? (_route = HttpContext.Features.Get<IRouteModel>() as T);

  IZeroContext _context;
  public IZeroContext Context => _context ?? (_context = HttpContext?.RequestServices?.GetService<IZeroContext>());
}