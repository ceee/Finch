using Microsoft.AspNetCore.Builder;

namespace zero.Backoffice;

public static class ZeroEndpointRouteBuilderExtensions
{
  public static void MapZeroApi(this IZeroEndpointRouteBuilder endpoints, string path = "/zero/api")
  {
    endpoints.MapFallbackToController(path.EnsureEndsWith('/') + "{**path}", "Index", "NotFoundZeroApi");
    //app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments(path + "/api"), app1 =>
    //{
    //  app1.UseEndpoints(endpoints =>
    //  {
    //    //IZeroOptions options = app.ApplicationServices.GetService<IZeroOptions>(); // TODO oO
    //    // see https://our.umbraco.com/documentation/reference/routing/custom-routes#where-to-put-your-routing-logic
    //    //string path = options.BackofficePath.EnsureStartsWith('/').TrimEnd('/');
    //    //endpoints.MapFallbackToController(path + "/{**path}", "Index", "ZeroIndex");

    //    //endpoints.MapControllers();
    //  });
    //});
  }
}
