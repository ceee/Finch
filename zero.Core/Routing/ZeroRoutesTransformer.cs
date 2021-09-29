using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace zero.Core.Routing
{
  public class ZeroRoutesTransformer : DynamicRouteValueTransformer
	{
    IRoutes Routes;
		IZeroContext Zero;

		public ZeroRoutesTransformer(IRoutes routes, IZeroContext zero)
    {
			Routes = routes;
			Zero = zero;
    }


		public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
		{
			IResolvedRoute route = await Routes.ResolveUrl(httpContext);

			(Zero as ZeroContext).SetRoute(route);

			if (route == null)
      {
				return null;
      }

			RouteProviderEndpoint endpoint = Routes.MapEndpoint(route);

			if (endpoint == null)
      {
				return null;
      }

			values["zero.route"] = route;
			values["controller"] = endpoint.Controller;
			values["action"] = endpoint.Action;
			return values;
		}
	}
}
