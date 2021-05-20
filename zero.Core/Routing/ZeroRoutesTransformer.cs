using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace zero.Core.Routing
{
  public class ZeroRoutesTransformer : DynamicRouteValueTransformer
	{
    IRoutes Routes;
		IZeroContext Context;

		public ZeroRoutesTransformer(IRoutes routes, IZeroContext context)
    {
			Routes = routes;
			Context = context;
    }


		public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
		{
			Console.WriteLine("ZeroRoutesTransformer");
			await Context.Resolve(httpContext);
			IResolvedRoute route = await Routes.ResolveUrl(httpContext);

			if (route == null)
      {
				return null;
      }

			RouteProviderEndpoint endpoint = Routes.MapEndpoint(route);

			values["zero.route"] = route;
			values["controller"] = endpoint.Controller;
			values["action"] = endpoint.Action;

			return values;
		}
	}
}
