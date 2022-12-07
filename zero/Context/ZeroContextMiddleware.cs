using Microsoft.AspNetCore.Http;

namespace zero.Context
{
  public class ZeroContextMiddleware
  {
    RequestDelegate _next;

    public ZeroContextMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IZeroContext zeroContext)
    {
      await zeroContext.Resolve(httpContext);
      await _next(httpContext);
    }
  }
}
