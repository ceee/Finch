using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Host;

namespace zero.Web.Middlewares
{
  public class ZeroMiddleware
  {
    readonly RequestDelegate Next;


    public ZeroMiddleware(RequestDelegate next)
    {
      Next = next;
    }


    public async Task Invoke(HttpContext httpContext, IZeroHost zeroHost)
    {
      await zeroHost.Initialize();

      if (httpContext == null)
      {
        throw new ArgumentNullException(nameof(httpContext));
      }

      IZeroContext context = await zeroHost.GetContext(httpContext);

      await Next(httpContext);
    }
  }
}
