using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Routing;

namespace zero.Core.Middlewares
{
  public class ApplicationContextMiddleware
  {
    RequestDelegate Next;


    public ApplicationContextMiddleware(RequestDelegate next)
    {
      Next = next;
    }


    public async Task Invoke(HttpContext httpContext, IApplicationContext appContext, IRoutes routes)
    {
      IApplication app = await appContext.Resolve(httpContext);

      //if (!appContext.IsBackofficeRequest(httpContext))
      //{
      //  IResolvedRoute route = await routes.ResolveUrl(app.Id, httpContext.Request.Path);
      //  httpContext.Response.ContentType = "application/json";
      //  await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(route));
      //  return;
      //}

      await Next(httpContext);
    }
  }
}
