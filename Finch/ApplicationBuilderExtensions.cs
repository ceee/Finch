using Microsoft.AspNetCore.Builder;

namespace Finch;

public static class ApplicationBuilderExtensions
{
  public static IApplicationBuilder UseFinch(this IApplicationBuilder app)
  {
    app.UseMiddleware<FinchContextMiddleware>();
    app.UseOutputCache();

    if (app is WebApplication webApplication)
    {
      webApplication.MapRazorPages();
      webApplication.MapControllers();
    }

    FinchBuilder.Modules.Configure(app, null, app.ApplicationServices);
    return app;
  }
}
