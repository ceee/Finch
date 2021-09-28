using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using zero.Core.Extensions;
using zero.Core.Options;
using zero.Core.Routing;
using zero.Web.Middlewares;

namespace zero.Web
{
  public static class ZeroApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseZero(this IApplicationBuilder app)
    {
      IZeroOptions options = app.ApplicationServices.GetService<IZeroOptions>();

      string path = options.BackofficePath.EnsureStartsWith('/').TrimEnd('/');

      if (!options.Routing.ErrorReexecutionPath.IsNullOrEmpty())
      {
        app.UseStatusCodePagesWithReExecute(options.Routing.ErrorReexecutionPath.EnsureStartsWith('/'), "?statusCode={0}");
      }

      app.UseMiddleware<ZeroContextMiddleware>();

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
          //endpoints.MapControllerRoute(
          //  name: "setup",
          //  pattern: path + "/setup",
          //  defaults: new
          //  {
          //    controller = "ZeroSetup",
          //    action = "Index"
          //  }
          //);

          //// routes for API
          //endpoints.MapControllerRoute(
          //  name: "api",
          //  pattern: path + "/api/{controller}/{action}/{id?}"
          //);

          // fallbacks for SPA
          endpoints.MapFallbackToController(path + "/{**path}", "Index", "ZeroBackoffice");
        });
      });

      return app;
    }


    public static IApplicationBuilder UseZeroRoutes(this IApplicationBuilder app)
    {
      IZeroOptions options = app.ApplicationServices.GetService<IZeroOptions>();

      return app.UseEndpoints(endpoints =>
      {
        endpoints.MapDynamicControllerRoute<ZeroRoutesTransformer>("{**url}", state: null, order: 10);
      });
    }
  }
}
