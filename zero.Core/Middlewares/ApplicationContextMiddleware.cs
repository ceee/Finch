using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace zero.Core.Middlewares
{
  public class ApplicationContextMiddleware
  {
    RequestDelegate Next;


    public ApplicationContextMiddleware(RequestDelegate next)
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
