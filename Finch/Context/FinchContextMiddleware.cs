using Microsoft.AspNetCore.Http;

namespace Finch.Context
{
  public class FinchContextMiddleware
  {
    RequestDelegate _next;

    public FinchContextMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IFinchContext finchContext)
    {
      await finchContext.Resolve(httpContext);
      await _next(httpContext);
    }
  }
}
