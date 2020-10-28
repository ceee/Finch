using Microsoft.AspNetCore.Http;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Routing;

namespace zero.Web.ViewHelpers
{
  public class ZeroViewContext : IZeroViewContext
  {
    IApplicationContext ApplicationContext;

    HttpContext HttpContext;


    public ZeroViewContext(IApplicationContext applicationContext, IHttpContextAccessor httpContextAccessor, IZeroMediaHelper mediaHelper)
    {
      ApplicationContext = applicationContext;
      HttpContext = httpContextAccessor.HttpContext;

      Application = ApplicationContext.App;
      AppId = ApplicationContext.AppId;
      if (HttpContext.Request.RouteValues.TryGetValue("zero.route", out object route))
      {
        Route = ((IResolvedRoute)route).Route;
      }
      Media = mediaHelper;
    }


    /// <inheritdoc />
    public IApplication Application { get; }

    /// <inheritdoc />
    public string AppId { get; }

    /// <inheritdoc />
    public IRoute Route { get; }

    /// <inheritdoc />
    public IZeroMediaHelper Media { get; }
  }


  public interface IZeroViewContext
  {
    /// <summary>
    /// Currently active application
    /// </summary>
    IApplication Application { get; }

    /// <summary>
    /// Currently active application id
    /// </summary>
    string AppId { get; }

    /// <summary>
    /// Matching path route
    /// </summary>
    IRoute Route { get; }

    /// <summary>
    /// Media helper functions
    /// </summary>
    public IZeroMediaHelper Media { get; }
  }
}
