using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace zero.Web.Middlewares
{
  public class PoweredByZeroMiddleware
  {
    readonly RequestDelegate Next;
    readonly PoweredByZeroMiddlewareOptions Options;

    public PoweredByZeroMiddleware(RequestDelegate next, PoweredByZeroMiddlewareOptions options)
    {
      Next = next;
      Options = options;
    }

    public Task Invoke(HttpContext httpContext)
    {
      if (Options.Enabled)
      {
        httpContext.Response.Headers["X-Powered-By"] = Options.HeaderValue;
      }

      return Next.Invoke(httpContext);
    }
  }


  public class PoweredByZeroMiddlewareOptions
  {
    public string HeaderValue { get; set; } = "Zero";

    public bool Enabled { get; set; } = true;
  }
}
