using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace zero.Core.Routing
{
  public class ZeroRoutesTransformer : DynamicRouteValueTransformer
	{
    IRouteResolver RouteResolver;

		public ZeroRoutesTransformer(IRouteResolver routeResolver)
    {
			RouteResolver = routeResolver;
    }


		public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
		{
			IResolvedRoute route = httpContext.Features.Get<IResolvedRoute>();
			
			if (route != null)
      {
				return null;
      }

			route = await RouteResolver.ResolveUrl(httpContext);

			if (route == null)
      {
				return null;
      }

			httpContext.Features.Set(route);

			RouteProviderEndpoint endpoint = RouteResolver.MapEndpoint(route);

			if (endpoint == null)
      {
				return null;
      }

			values["controller"] = endpoint.Controller;
			values["action"] = endpoint.Action;
			return values;
		}
	}
}
