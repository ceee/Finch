using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using zero.Core.Extensions;
using zero.Core.Middlewares;
using zero.Core.Options;
using zero.Web.Routing;

namespace zero.Web
{
  public static class ZeroApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseZero(this IApplicationBuilder app)
    {
      IZeroOptions options = app.ApplicationServices.GetService<IZeroOptions>();

      string path = options.BackofficePath.EnsureStartsWith('/').TrimEnd('/');

      app.UseStaticFiles();

      app.UseMiddleware<ZeroContextMiddleware>();

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


      app.UseEndpoints(endpoints =>
      {
        endpoints.MapDynamicControllerRoute<ZeroRoutesTransformer>("{**url}");
      });

      return app;
    }
  }
}
