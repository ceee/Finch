using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Finch;

public static class ApplicationBuilderExtensions
{
  public static IApplicationBuilder UseFinch(this IApplicationBuilder app)
  {
    app.UseMiddleware<FinchContextMiddleware>();
    app.UseResponseCaching();
    app.UseOutputCache();

    if (app is WebApplication webApplication)
    {
      webApplication.MapRazorPages();
      webApplication.MapControllers();
    }

    FinchBuilder.Modules.Configure(app, app as IEndpointRouteBuilder, app.ApplicationServices);
    return app;
  }
}
