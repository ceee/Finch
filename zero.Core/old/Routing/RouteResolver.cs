using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Exceptions.Documents.Indexes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Routing
{
  public class RouteResolver : Routes, IRouteResolver
  {
    protected IApplicationResolver AppResolver { get; set; }
    protected IZeroOptions Options { get; set; }


    public RouteResolver(IZeroContext context, IZeroStore store, ILogger<Routes> logger, IEnumerable<IRouteProvider> providers, IApplicationResolver appResolver) : base(context, store, logger, providers)
    {
      AppResolver = appResolver;
      Options = context.Options;
    }


    /// <inheritdoc />
    public async Task<IRouteModel> ResolveUrl(HttpContext context, Application application, string path)
    {
      IZeroDocumentSession session = Store.Session();

      path = path.Length > 1 ? path.TrimEnd(PATH_SEPERATOR) : path;

      string[] pathParts = path.Trim(PATH_SEPERATOR).Split(PATH_SEPERATOR);
      string[] parts = new string[pathParts.Length];

      int min = parts.Length;
      foreach (string pathPart in pathParts)
      {
        for (int i = 0; i < min; i++)
        {
          parts[i] += PATH_SEPERATOR + pathPart;
        }
        min -= 1;
      }

      IList<Route> routes = null;

      try
      {
        routes = await session.Query<Route, Routes_ForResolver>()
          .Where(x => (!x.AllowSuffix && x.Url == path) || (x.AllowSuffix && x.Url.In(parts)))
          .Include("References[].Id")
          .Include("Dependencies")
          .ToListAsync();

        routes = routes.Where(x => !x.Param<bool>(PageRouteProvider.PAGE_IS_FOLDER)).ToList();
      }
      catch (IndexDoesNotExistException ex)
      {
        Logger.LogError(ex, "Indexes have not been created yet");
        return null;
      }

      Route route = routes.FirstOrDefault();

      // try to get the best matching path when multiple routes are found
      // our assumption is that the best path is those with the longest path parts separated by a slash
      // if we still get multiple routes with equal part counts then we prefer those which do not allow a suffix
      if (routes.Count > 1)
      {
        int maxPathParts = routes.Max(x => x.Url.Count(u => u == PATH_SEPERATOR));
        IEnumerable<Route> longestRoutes = routes.Where(x => maxPathParts == x.Url.Count(u => u == PATH_SEPERATOR)).OrderBy(x => x.AllowSuffix);

        if (longestRoutes.Count() > 1) 
        {
          Logger.LogWarning("Multiple routes {routes} were found for {path}", longestRoutes.Select(x => x.Id), path);
        }

        route = longestRoutes.FirstOrDefault();
      }

      if (route == null)
      {
        return null;
      }

      return await ResolveRouteInternal(new RouteResponse()
      {
        Route = route,
        App = application,
        HttpContext = context,
        Path = path
      });
    }


    /// <inheritdoc />
    public async Task<IRouteModel> ResolveUrl(HttpContext context)
    {
      if (!Options.SetupCompleted || context.IsBackofficeRequest(Options.BackofficePath))
      {
        return null;
      }

      Application app = await AppResolver.ResolveFromRequest(context);
      string path = context.Request.Path;

      if (app == null)
      {
        return null;
      }

      return await ResolveUrl(context, app, path);
    }


    /// <inheritdoc />
    public RouteEndpoint MapEndpoint(IRouteModel route)
    {
      if (TryGetProvider(route.Route.ProviderAlias, out IRouteProvider provider))
      {
        return provider.Map(GetContext(), route);
      }

      return null;
    }


    /// <summary>
    /// Call the provider which can resolve the route
    /// </summary>
    async Task<IRouteModel> ResolveRouteInternal(RouteResponse response)
    {
      if (TryGetProvider(response.Route.ProviderAlias, out IRouteProvider provider))
      {
        return await provider.Model(GetContext(), response);
      }

      return null;
    }
  }


  public interface IRouteResolver
  {
    /// <summary>
    /// Resolve an URL from the specified app and the path
    /// </summary>
    Task<IRouteModel> ResolveUrl(HttpContext context, Application application, string path);

    /// <summary>
    /// Resolve an URL from an http context
    /// </summary>
    Task<IRouteModel> ResolveUrl(HttpContext context);

    /// <summary>
    /// Get endpoint the route maps to
    /// </summary>
    RouteEndpoint MapEndpoint(IRouteModel route);
  }
}
