using Microsoft.AspNetCore.Builder;

namespace zero.Routing;

public static class ZeroEndpointRouteBuilderExtensions
{
  public static IZeroEndpointRouteBuilder MapZeroBackoffice(this IZeroEndpointRouteBuilder endpoints, string path = "/zero")
  {
    //IZeroOptions options = app.ApplicationServices.GetService<IZeroOptions>(); // TODO oO
    // see https://our.umbraco.com/documentation/reference/routing/custom-routes#where-to-put-your-routing-logic
    //string path = options.BackofficePath.EnsureStartsWith('/').TrimEnd('/');

    endpoints.MapFallbackToController(path + "/{**path}", "Index", "ZeroIndex");
    return endpoints;
  }


  public static IZeroEndpointRouteBuilder MapZeroRoutes(this IZeroEndpointRouteBuilder endpoints)
  {
    endpoints.MapDynamicControllerRoute<ZeroRoutesTransformer>("/{**url}", state: null, order: 10);
    return endpoints;
  }
}
