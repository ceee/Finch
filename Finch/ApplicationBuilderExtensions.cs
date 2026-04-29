using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Finch;

public static class ApplicationBuilderExtensions
{
  public static IApplicationBuilder UseFinch(this IApplicationBuilder app)
  {
    app.UseMiddleware<FinchContextMiddleware>();
    app.UseResponseCaching();

    IHostEnvironment env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();
    if (!env.IsDevelopment())
    {
      app.UseOutputCache();
    }

    if (app is WebApplication webApplication)
    {
      webApplication.MapRazorPages();
      webApplication.MapControllers();
    }

    FinchBuilder.Modules.Configure(app, app as IEndpointRouteBuilder, app.ApplicationServices);
    return app;
  }
}
