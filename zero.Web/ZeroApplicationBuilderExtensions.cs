using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using zero.Core;
using zero.Core.Extensions;
using zero.Core.Middlewares;
using zero.Core.Options;

namespace zero.Web
{
  public static class ZeroApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseZero(this IApplicationBuilder app)
    {
      IZeroOptions options = app.ApplicationServices.GetService<IZeroOptions>();

      string path = options.BackofficePath.EnsureStartsWith('/').TrimEnd('/');

      app.UseStaticFiles();

      app.UseMiddleware<ApplicationContextMiddleware>();

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

      //app.UseSpa(spa =>
      //{
      //  spa.Options.SourcePath = webUiPath;
      //  spa.UseProxyToSpaDevelopmentServer("http://localhost:8999");
      //});

#pragma warning disable CS0618
      app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions()
      {
        HotModuleReplacement = true,
        ProjectPath = webUiPath,
        HotModuleReplacementServerPort = 8999
      });
      #pragma warning restore CS0618

      return app;
    }
  }
}
