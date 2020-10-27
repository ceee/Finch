using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using VueCliMiddleware;
using zero.Core.Extensions;
using zero.Core.Middlewares;
using zero.Core.Options;
using zero.Web.Routing;

namespace zero.Web
{
  public static class ZeroApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseZero(this IApplicationBuilder app, string devPath = null)
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

          if (devPath != null)
          {
            endpoints.MapFallback(path + "/vue-cli/{**path}", CreateVueProxyDelegate(endpoints, new SpaOptions { SourcePath = devPath }));
          }

          // fallbacks for SPA
          endpoints.MapFallbackToController(path + "/{**path}", "Index", "Index");
        });
      });


      app.UseEndpoints(endpoints =>
      {
        endpoints.MapDynamicControllerRoute<ZeroRoutesTransformer>("{**url}");
      });

      return app;
    }


    static RequestDelegate CreateVueProxyDelegate(IEndpointRouteBuilder endpoints, SpaOptions options = null)
    {
      var app = endpoints.CreateApplicationBuilder();
      app.Use(next => context =>
      {
        context.SetEndpoint(null);
        return next(context);
      });

      app.UseSpa(opt =>
      {
        if (options != null)
        {
          opt.Options.DefaultPage = options.DefaultPage;
          opt.Options.DefaultPageStaticFileOptions = options.DefaultPageStaticFileOptions;
          opt.Options.SourcePath = options.SourcePath;
          opt.Options.StartupTimeout = options.StartupTimeout;
        }

        opt.UseVueCli();
      });

      return app.Build();
    }


    public static IApplicationBuilder UseZeroDevEnvironment(this IApplicationBuilder app)
    {
      //IZeroOptions options = app.ApplicationServices.GetService<IZeroOptions>();
      //string zeroPath = options.BackofficePath.EnsureStartsWith('/').TrimEnd('/');


      //string webUiPath = path ?? Path.Combine(Environment.CurrentDirectory, "..", "zero.Web.UI"); // TODO dynPATH

      // map backoffice
      //app.UseWhen(ctx => ctx.Request.Path.ToString().StartsWith(zeroPath), builder =>
      //{
      //  builder.UseRouting();


      //});

      //#pragma warning disable CS0618
      //app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions()
      //{
      //  HotModuleReplacement = true,
      //  ProjectPath = webUiPath,
      //  HotModuleReplacementServerPort = 8999,
      //  EnvironmentVariables = new Dictionary<string, string>()
      //  {
      //    { "ZERO", "hi from zero 0.1" }
      //  }
      //});
      //#pragma warning restore CS0618

      return app;
    }
  }
}
