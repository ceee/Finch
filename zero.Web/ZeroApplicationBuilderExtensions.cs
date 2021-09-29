using Microsoft.AspNetCore.Builder;
using zero.Web.Middlewares;

namespace zero.Web
{
  public static class ZeroApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseZero(this IApplicationBuilder app)
    {
      app.UseMiddleware<ZeroContextMiddleware>();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      return app;
    }
  }
}
