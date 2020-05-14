using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using zero.Core.Options;

namespace zero.Web
{
  public class ZeroMiddleware
  {
    private RequestDelegate Next { get; set; }
    private IZeroOptions Options { get; set; }


    public ZeroMiddleware(RequestDelegate next, IZeroOptions options)
    {
      Next = next;
      Options = options; 
    }


    public async Task Invoke(HttpContext httpContext)
    {
      await httpContext.Response.WriteAsync("from zero");
      //var context = null; // new AspNetCoreDashboardContext(_storage, _options, httpContext);
      //var findResult = Routes.FindDispatcher(httpContext.Request.Path.Value);

      //if (findResult == null)
      //{
      //  await Next.Invoke(httpContext);
      //  return;
      //}

      // ReSharper disable once LoopCanBeConvertedToQuery
      //foreach (var filter in Options.Authorization)
      //{
      //  if (!filter.Authorize(context))
      //  {
      //    var isAuthenticated = httpContext.User?.Identity?.IsAuthenticated;

      //    httpContext.Response.StatusCode = isAuthenticated == true
      //        ? (int)HttpStatusCode.Forbidden
      //        : (int)HttpStatusCode.Unauthorized;

      //    return;
      //  }
      //}

      //if (!_options.IgnoreAntiforgeryToken)
      //{
      //  var antiforgery = httpContext.RequestServices.GetService<IAntiforgery>();

      //  if (antiforgery != null)
      //  {
      //    var requestValid = await antiforgery.IsRequestValidAsync(httpContext);

      //    if (!requestValid)
      //    {
      //      // Invalid or missing CSRF token
      //      httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
      //      return;
      //    }
      //  }
      //}

      //context.UriMatch = findResult.Item2;

      //await findResult.Item1.Dispatch(context);
    }
  }
}