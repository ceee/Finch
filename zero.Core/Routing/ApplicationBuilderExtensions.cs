using Microsoft.AspNetCore.Builder;

namespace zero.Routing;

public static class ApplicationBuilderExtensions
{
  public static IApplicationBuilder UseZeroRouting(this IApplicationBuilder app)
  {
    app.UseStaticFiles();
    app.UseRouting();
    return app;
  }
}
