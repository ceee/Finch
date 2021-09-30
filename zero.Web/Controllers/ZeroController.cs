using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using zero.Core;
using zero.Core.Entities;
using zero.Core.Routing;

namespace zero.Web.Controllers
{
  public abstract class ZeroController : ZeroController<DefaultResolvedRoute> { }


  public abstract class ZeroController<T> : Controller where T : class, IResolvedRoute
  {
    public virtual Application Application => Context?.Application;

    T _route;
    public virtual T Route => _route ?? (_route = HttpContext.Features.Get<IResolvedRoute>() as T);

    IZeroContext _context;
    public IZeroContext Context => _context ?? (_context = HttpContext?.RequestServices?.GetService<IZeroContext>());
  }
}
