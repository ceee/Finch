using Microsoft.AspNetCore.Builder;

namespace zero;

public static class ApplicationBuilderExtensions
{
  public static IApplicationBuilder UseZero(this IApplicationBuilder app)
  {
    app.UseMiddleware<ZeroContextMiddleware>();
    ZeroBuilder.Modules.Configure(app, null, app.ApplicationServices);
    return app;
  }
}
