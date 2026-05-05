using Microsoft.AspNetCore.Http;

namespace Mixtape.Context
{
  public class MixtapeContextMiddleware(RequestDelegate next)
  {
    public async Task Invoke(HttpContext httpContext, IMixtapeContext mixtapeContext)
    {
      await mixtapeContext.Resolve(httpContext);
      await next(httpContext);
    }
  }
}
