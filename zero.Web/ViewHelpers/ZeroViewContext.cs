using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Routing;

namespace zero.Web.ViewHelpers
{
  public class ZeroViewContext : IZeroViewContext
  {
    /// <inheritdoc />
    public IApplication Application { get; private set; }

    /// <inheritdoc />
    public string AppId { get; private set; }

    /// <inheritdoc />
    public IRoute Route { get; private set; }

    /// <inheritdoc />
    public IResolvedRoute ResolvedRoute { get; private set; }

    /// <inheritdoc />
    public IZeroMediaHelper Media { get; private set; }


    protected IZeroContext Context { get; private set; }


    public ZeroViewContext(IZeroContext context, IZeroMediaHelper mediaHelper)
    {
      Context = context;
      Media = mediaHelper;
    }


    /// <inheritdoc />
    public Task Resolve(HttpContext context)
    {
      Application = Context.App;
      AppId = Context.AppId;

      if (context?.Request == null || Context.IsBackofficeRequest)
      {
        return Task.CompletedTask;
      }

      if (context.Request.RouteValues.TryGetValue("zero.route", out object route))
      {
        ResolvedRoute = (IResolvedRoute)route;
        Route = ResolvedRoute.Route;
      }

      return Task.CompletedTask;
    }
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
    /// Matching resolved route
    /// </summary>
    IResolvedRoute ResolvedRoute { get; }

    /// <summary>
    /// Media helper functions
    /// </summary>
    public IZeroMediaHelper Media { get; }

    /// <summary>
    /// Resolve view context
    /// </summary>
    Task Resolve(HttpContext context);
  }
}
