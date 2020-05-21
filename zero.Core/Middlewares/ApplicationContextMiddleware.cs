using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using zero.Core.Api;

namespace zero.Core.Middlewares
{
  public class ApplicationContextMiddleware
  {
    RequestDelegate Next;


    public ApplicationContextMiddleware(RequestDelegate next)
    {
      Next = next;
    }


    public async Task Invoke(HttpContext httpContext, IApplicationContext appContext)
    {
      await appContext.Resolve(httpContext);
      await Next(httpContext);
    }
  }
}
