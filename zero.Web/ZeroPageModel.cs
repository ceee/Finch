using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using zero.Applications;
using zero.Context;
using zero.Routing;

namespace zero.Web;

public abstract class ZeroPageModel<T> : ZeroPageModel where T : class, IRouteModel
{
  /// <summary>
  /// Resolved route
  /// </summary>
  public virtual T Route => _route ?? (_route = HttpContext.Features.Get<IRouteModel>() as T);
  T _route;
}

public abstract class ZeroPageModel : PageModel
{
  /// <summary>
  /// Get acces to the zero context for this request
  /// </summary>
  public IZeroContext ZeroContext => _zeroContext ?? (_zeroContext = HttpContext?.RequestServices?.GetService<IZeroContext>());
  IZeroContext _zeroContext;

  /// <summary>
  /// Resolved application
  /// </summary>
  public virtual Application Application => ZeroContext?.Application;
}