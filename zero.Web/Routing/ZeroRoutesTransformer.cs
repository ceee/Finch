using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Routing;

namespace zero.Web.Routing
{
  public class ZeroRoutesTransformer : DynamicRouteValueTransformer
	{
		IRoutes Routes;
		IApplicationContext Context;

		public ZeroRoutesTransformer(IRoutes routes, IApplicationContext context)
    {
			Routes = routes;
			Context = context;
    }


		public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
		{
			if (Context.IsBackofficeRequest(httpContext))
      {
				return null;
      }

			IResolvedRoute route = await Routes.ResolveUrl(httpContext);

			if (route == null)
      {
				return null;
      }

			RouteProviderEndpoint endpoint = Routes.MapEndpoint(route);

			values["zero.app"] = await Context.ResolveFromRequest(httpContext);
			values["zero.route"] = route;
			values["controller"] = endpoint.Controller;
			values["action"] = endpoint.Action;

			return values;
		}
	}
}
