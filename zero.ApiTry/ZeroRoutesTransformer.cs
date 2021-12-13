using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace zero.ApiTry;

public class ZeroRoutesTransformer : DynamicRouteValueTransformer
{
	public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
	{
    if (httpContext.Request.Path.StartsWithSegments("/zero"))
    {
      return values;
    }

    values["controller"] = "Frontend";
		values["action"] = "Index";
		return values;
	}


  public override ValueTask<IReadOnlyList<Endpoint>> FilterAsync(HttpContext httpContext, RouteValueDictionary values, IReadOnlyList<Endpoint> endpoints)
  {
    return base.FilterAsync(httpContext, values, endpoints);
  }
}