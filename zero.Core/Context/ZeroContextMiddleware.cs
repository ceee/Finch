using Microsoft.AspNetCore.Http;

namespace zero.Context
{
  public class ZeroContextMiddleware
  {
    RequestDelegate Next;

    public ZeroContextMiddleware(RequestDelegate next)
    {
      Next = next;
    }

    public async Task Invoke(HttpContext httpContext, IZeroContext zeroContext)
    {
      await zeroContext.Resolve(httpContext);
      await Next(httpContext);
    }
  }
}
