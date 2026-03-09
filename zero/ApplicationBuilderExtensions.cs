using Microsoft.AspNetCore.Builder;

namespace zero;

public static class ApplicationBuilderExtensions
{
  public static IApplicationBuilder UseZero(this IApplicationBuilder app)
  {
    app.UseMiddleware<ZeroContextMiddleware>();
    app.UseOutputCache();

    if (app is WebApplication webApplication)
    {
      webApplication.MapRazorPages();
      webApplication.MapControllers();
    }

    ZeroBuilder.Modules.Configure(app, null, app.ApplicationServices);
    return app;
  }
}
