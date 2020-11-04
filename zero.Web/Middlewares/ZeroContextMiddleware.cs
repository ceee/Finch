using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using zero.Core;
using zero.Web.ViewHelpers;

namespace zero.Web.Middlewares
{
  public class ZeroContextMiddleware
  {
    RequestDelegate Next;

    public ZeroContextMiddleware(RequestDelegate next)
    {
      Next = next;
    }

    public async Task Invoke(HttpContext httpContext, IZeroContext zeroContext, IZeroViewContext viewContext)
    {
      await zeroContext.Resolve(httpContext);
      await viewContext.Resolve(httpContext);
      await Next(httpContext);
    }
  }
}
