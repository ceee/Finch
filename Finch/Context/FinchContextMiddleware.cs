using Microsoft.AspNetCore.Http;

namespace Finch.Context
{
  public class FinchContextMiddleware(RequestDelegate next)
  {
    public async Task Invoke(HttpContext httpContext, IFinchContext finchContext)
    {
      await finchContext.Resolve(httpContext);
      await next(httpContext);
    }
  }
}
