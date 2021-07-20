using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace zero.Core.Routing
{
  public class ZeroRoutesTransformer : DynamicRouteValueTransformer
	{
    IRoutes Routes;

		public ZeroRoutesTransformer(IRoutes routes)
    {
			Routes = routes;
    }


		public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
		{
			IResolvedRoute route = await Routes.ResolveUrl(httpContext);

			if (route == null)
      {
				return HandleFail(httpContext, values);
      }

			RouteProviderEndpoint endpoint = Routes.MapEndpoint(route);

			if (endpoint == null)
      {
				return HandleFail(httpContext, values);
      }

			values["zero.route"] = route;
			values["controller"] = endpoint.Controller;
			values["action"] = endpoint.Action;
			return values;
		}


		RouteValueDictionary HandleFail(HttpContext httpContext, RouteValueDictionary values)
    {
			NotFoundRoute notFound = Routes.NotFound(httpContext);

			if (notFound != null)
      {
				httpContext.Response.StatusCode = 404;
				values["zero.route"] = notFound;
				values["controller"] = notFound.Controller;
				values["action"] = notFound.Action;
				return values;
      }
			return null;
    }
	}
}
