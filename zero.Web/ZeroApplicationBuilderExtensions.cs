using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using zero.Core;
using zero.Core.Extensions;

namespace zero.Web
{
  public static class ZeroApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseZero(this IApplicationBuilder app)
    {
      ZeroOptions options = app.ApplicationServices.GetService<IOptionsMonitor<ZeroOptions>>().CurrentValue;

      string path = options.BackofficePath.EnsureStartsWith('/').TrimEnd('/');

      app.UseStaticFiles();

      // map backoffice
      app.UseWhen(ctx => ctx.Request.Path.ToString().StartsWith(path), builder =>
      {
        builder.UseRouting();

        builder.UseAuthentication();
        builder.UseAuthorization();

        //builder.UseMiddleware<ZeroMiddleware>(options);

        builder.UseEndpoints(endpoints =>
        {
          // setup route
          endpoints.MapControllerRoute(
            name: "setup",
            pattern: path + "/setup",
            defaults: new
            {
              controller = "Setup",
              action = "Index"
            }
          );

          // routes for API
          endpoints.MapControllerRoute(
            name: "api",
            pattern: path + "/api/{controller}/{action}/{id?}"
          );

          // fallbacks for SPA
          endpoints.MapFallbackToController(path + "/{**path}", "Index", "Index");
        });
      });

      return app;
    }


    public static IApplicationBuilder UseZeroDevEnvironment(this IApplicationBuilder app)
    {
      app.UseDeveloperExceptionPage();

      string webUiPath = Path.Combine(Environment.CurrentDirectory, "..", "zero.Web.UI");

      #pragma warning disable CS0618
      app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions()
      {
        HotModuleReplacement = true,
        ProjectPath = webUiPath,
      });
      #pragma warning restore CS0618

      return app;
    }
  }
}
