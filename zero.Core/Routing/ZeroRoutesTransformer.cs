using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace zero.Routing;

public class ZeroRoutesTransformer : DynamicRouteValueTransformer
{
  readonly IRouteResolver RouteResolver;

	public ZeroRoutesTransformer(IRouteResolver routeResolver)
  {
		RouteResolver = routeResolver;
  }


	public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
	{
		IRouteModel route = httpContext.Features.Get<IRouteModel>();
			
		if (route != null)
    {
			return null;
    }

		route = await RouteResolver.ResolveUrl(httpContext);

		if (route == null)
    {
			return null;
    }

		httpContext.Features.Set<IRouteModel>(route);

		RouteEndpoint endpoint = RouteResolver.MapEndpoint(route);

		if (endpoint == null)
    {
			return null;
    }

		values["controller"] = endpoint.Controller;
		values["action"] = endpoint.Action;
		return values;
	}
}
