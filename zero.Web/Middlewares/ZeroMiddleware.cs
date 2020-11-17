using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Host;
using zero.Core.Routing;

namespace zero.Web.Middlewares
{
  public class ZeroMiddleware
  {
    readonly RequestDelegate Next;
    readonly IZeroHost Host;
    readonly IRoutes Routes;


    public ZeroMiddleware(RequestDelegate next, IZeroHost zeroHost, IRoutes routes)
    {
      Next = next;
      Host = zeroHost;
      Routes = routes;
    }


    public async Task Invoke(HttpContext httpContext)
    {
      await Host.Initialize();

      if (httpContext == null)
      {
        throw new ArgumentNullException(nameof(httpContext));
      }

      IZeroContext context = await Host.GetContext(httpContext);

      if (!context.IsBackofficeRequest)
      {
        IResolvedRoute route = await Routes.ResolveUrl(httpContext);

        if (route != null)
        {
          context.SetRoute(route);

          RouteProviderEndpoint endpoint = Routes.MapEndpoint(route);

          httpContext.Request.RouteValues["controller"] = endpoint.Controller;
          httpContext.Request.RouteValues["action"] = endpoint.Action;
        }
      }

      await Next(httpContext);
    }
  }
}
