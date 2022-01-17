using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace zero.Routing;

public class ZeroRoutesTransformer : DynamicRouteValueTransformer
{
  readonly IRouteResolver RouteResolver;
	readonly IMediaFileSystem MediaFileSystem;
	readonly ILogger<ZeroRoutesTransformer> Logger;

	public ZeroRoutesTransformer(IRouteResolver routeResolver, IMediaFileSystem mediaFileSystem, ILogger<ZeroRoutesTransformer> logger)
  {
		RouteResolver = routeResolver;
		MediaFileSystem = mediaFileSystem;
		Logger = logger;
	}


	public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
	{
		if (State == null || State is not ZeroRoutesTransformerType)
		{
			throw new ArgumentNullException(nameof(State), "Pass a ZeroRoutesTransformerType instance to the state parameter in the dynamic route transformer");
		}

		ZeroRoutesTransformerType type = (ZeroRoutesTransformerType)State;

		IRouteEndpoint endpoint = httpContext.Features.Get<IRouteEndpoint>();
		IRouteModel route = httpContext.Features.Get<IRouteModel>();

		// check if a resolved endpoint routes to a Razor Page.
		// if the Page transformer can't find an endpoint it exits as the Controller transformer already ran before.
		if (type == ZeroRoutesTransformerType.Page)
    {
			if (endpoint is PageRouteEndpoint)
			{
				return Apply(httpContext, values, endpoint, type);
			}

			return null;
		}

		// a route was already set, therefore we prevent another execution
		if (route != null)
		{
			return null;
		}

		// do not continue if it is a media path
		if (MediaFileSystem.IsMediaPath(httpContext.Request.Path))
    {
			return null;
    }

		// resolve route from URL
		route = await RouteResolver.ResolveUrl(httpContext);

		if (route == null)
    {
			return null;
    }

		httpContext.Features.Set(route);

		// map the route to an endpoint
		// an endpoint can be a controller action or a Razor page.
		// in case it is a Razor page we exit here and let the transformer run again within the page context (MapDynamicPageRoute).
		endpoint = RouteResolver.MapEndpoint(route);

		if (endpoint == null)
    {
			return null;
    }

		httpContext.Features.Set(endpoint);

		if (endpoint is PageRouteEndpoint)
    {
			return null;
    }

		return Apply(httpContext, values, endpoint, type);
	}


	protected virtual RouteValueDictionary Apply(HttpContext httpContext, RouteValueDictionary values, IRouteEndpoint endpoint, ZeroRoutesTransformerType type)
  {
		endpoint.Apply(values);

		HashSet<string> logValues = new();

		foreach ((string key, object value) in values)
		{
			if (key != "url" && value != null && (value is not string || ((string)value).HasValue()))
			{
				logValues.Add(key + ": " + value);
			}
		}

		Logger.LogDebug("Routed {url} to [{values}] (transformer: {type})", httpContext.Request.Path, logValues, type);

		return values;
	}
}


public enum ZeroRoutesTransformerType
{
	Controller = 0,
	Page = 1
}