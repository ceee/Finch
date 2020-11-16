using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using zero.Core;
using zero.Core.Extensions;
using zero.Core.Options;
using zero.Web.Middlewares;
using zero.Web.Routing;

namespace zero.Web
{
  public static class ZeroApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseZero(this IApplicationBuilder app)
    {
      IZeroOptions options = app.ApplicationServices.GetService<IZeroOptions>();

      string path = options.BackofficePath.EnsureStartsWith('/').TrimEnd('/');

      app.UseMiddleware<PoweredByZeroMiddleware>();
      app.UseMiddleware<ZeroMiddleware>();

      // map backoffice
      app.UseWhen(ctx => ctx.Request.Path.ToString().StartsWith(path), builder =>
      {
        builder.UseRouting();

        builder.UseAuthentication();
        builder.UseAuthorization();

        builder.UseEndpoints(endpoints =>
        {
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


    public static IEndpointConventionBuilder MapZero(this IEndpointRouteBuilder endpoints)
    {
      return MapZeroCore(endpoints, "/zero/{**path}", new ZeroEndpointOptions());
    }


    public static IEndpointConventionBuilder MapZero(this IEndpointRouteBuilder endpoints, string pattern)
    {
      return MapZeroCore(endpoints, pattern, new ZeroEndpointOptions());
    }


    public static IEndpointConventionBuilder MapZero(this IEndpointRouteBuilder endpoints, string pattern, ZeroEndpointOptions options)
    {
      return MapZeroCore(endpoints, pattern, options);
    }


    //public static IEndpointRouteBuilder MapZeroRoutes(this IEndpointRouteBuilder endpoints)
    //{
    //  endpoints.MapDynamicControllerRoute<ZeroRoutesTransformer>("{**url}");
    //  return endpoints;
    //}


    static IEndpointConventionBuilder MapZeroCore(IEndpointRouteBuilder endpoints, string pattern, ZeroEndpointOptions options)
    {
      if (endpoints.ServiceProvider.GetService<IZeroContext>() == null)
      {
        throw new InvalidOperationException();
      }

      object[] args = options != null ? new[] { Options.Create(options) } : Array.Empty<object>();

      RequestDelegate backofficePipeline = endpoints.CreateApplicationBuilder()
         .UseMiddleware<ZeroMiddleware>(args)
         .Build();

      RequestDelegate frontendPipeline = endpoints.CreateApplicationBuilder()
         .UseMiddleware<ZeroMiddleware>(args)
         .Build();

      endpoints.map(frontendPipeline);

      return endpoints.Map(pattern, backofficePipeline).WithDisplayName("Zero");
    }
  }
}
