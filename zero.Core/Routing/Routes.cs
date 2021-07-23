using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Client.Exceptions.Documents.Indexes;
using System;
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
  public class Routes : IRoutes
  {
    public const char PATH_SEPERATOR = '/';

    protected IZeroStore Store { get; set; }
    protected ILogger<Routes> Logger { get; set; }
    protected IEnumerable<IRouteProvider> Providers { get; set; }
    protected IApplicationResolver AppResolver { get; set; }
    protected IZeroOptions Options { get; set; }


    public Routes(IZeroStore store, ILogger<Routes> logger, IEnumerable<IRouteProvider> providers, IApplicationResolver appResolver, IZeroOptions options)
    {
      Store = store;
      Logger = logger;
      Providers = providers;
      AppResolver = appResolver;
      Options = options;
    }


    /// <inheritdoc />
    public async Task<string> GetUrl<T>(T model, object parameters = null) => (await GetRoute(model, parameters))?.Url;


    /// <inheritdoc />
    public async Task<string> GetUrl<T>(string id, object parameters = null) where T : ZeroIdEntity => (await GetRoute<T>(id, parameters))?.Url;


    /// <inheritdoc />
    public async Task<Dictionary<T, string>> GetUrls<T>(IEnumerable<T> models, object parameters = null) => (await GetRoutes(models, parameters)).ToDictionary(x => x.Key, x => x.Value?.Url);


    /// <inheritdoc />
    public async Task<Route> GetRoute<T>(string id, object parameters = null) where T : ZeroIdEntity
    {
      if (id.IsNullOrEmpty())
      {
        return null;
      }

      Type type = typeof(T);
      IRouteProvider routeProvider = Providers.FirstOrDefault(x => x.AffectedTypes.Any(t => t.IsAssignableFrom(type)));

      if (routeProvider == null)
      {
        return null;
      }

      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      T model = await session.LoadAsync<T>(id);

      if (model == null)
      {
        return null;
      }

      return await routeProvider.GetRoute(session, model, parameters);
    }


    /// <inheritdoc />
    public async Task<Route> GetRoute<T>(T model, object parameters = null)
    {
      if (model == null)
      {
        return null;
      }

      Type type = model.GetType();
      IRouteProvider routeProvider = Providers.FirstOrDefault(x => x.AffectedTypes.Any(t => t.IsAssignableFrom(type)));

      if (routeProvider == null)
      {
        return null;
      }

      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return await routeProvider.GetRoute(session, model, parameters);
    }


    /// <inheritdoc />
    public async Task<Dictionary<T, Route>> GetRoutes<T>(IEnumerable<T> models, object parameters = null)
    {
      if (!models.Any())
      {
        return new();
      }

      T firstExistingModel = models.FirstOrDefault(x => x != null);

      if (firstExistingModel == null)
      {
        return new();
      }

      Type type = firstExistingModel.GetType();
      IRouteProvider routeProvider = Providers.FirstOrDefault(x => x.AffectedTypes.Any(t => t.IsAssignableFrom(type)));

      if (routeProvider == null)
      {
        return null;
      }

      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return (await routeProvider.GetRoutes(session, models.Select(x => (object)x), parameters)).ToDictionary(x => (T)x.Key, x => x.Value);
    }


    /// <inheritdoc />
    public async Task<IResolvedRoute> ResolveUrl(Application application, string path)
    {
      path = path.Length > 1 ? path.TrimEnd(PATH_SEPERATOR) : path;

      using IAsyncDocumentSession session = Store.OpenAsyncSession(application.Database);

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

      return await ResolveRouteInternal(session, route);
    }


    /// <inheritdoc />
    public async Task<IResolvedRoute> ResolveUrl(HttpContext context)
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

      return await ResolveUrl(app, path);
    }


    /// <inheritdoc />
    public async Task<IResolvedRoute> ResolveRoute(Route route)
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return await ResolveRouteInternal(session, route);
    }


    /// <inheritdoc />
    public NotFoundRoute NotFound(HttpContext context)
    {
      if (!Options.SetupCompleted || context.IsBackofficeRequest(Options.BackofficePath) || Options.Routing.NotFoundEndpoint == null)
      {
        return null;
      }

      return new NotFoundRoute(context)
      {
        Controller = Options.Routing.NotFoundEndpoint.Controller,
        Action = Options.Routing.NotFoundEndpoint.Action
      };
    }


    /// <inheritdoc />
    public async Task RebuildAllRoutes()
    {
      int count = 0;

      using IAsyncDocumentSession coreSession = Store.OpenCoreSession();
      List<Application> apps = await coreSession.Query<Application>().ToListAsync();

      foreach (Application app in apps)
      {
        using IAsyncDocumentSession session = Store.OpenAsyncSession(app.Database);
        session.Advanced.MaxNumberOfRequestsPerSession = 1000;

        foreach (IRouteProvider provider in Providers)
        {
          // get all routes for this provider
          IList<Route> routes = await provider.GetAllRoutes(session);

          // delete all registered routes in the database for this provider
          await Store.PurgeAsync<Route>(app.Database, $"where {nameof(Route.ProviderAlias)} = $alias", new Raven.Client.Parameters()
          {
            { "alias", provider.Alias }
          });

          // store new routes
          using (BulkInsertOperation bulkInsert = Store.BulkInsert(app.Database))
          {
            foreach (Route route in routes)
            {
              await bulkInsert.StoreAsync(route, route.Id);
              count += 1;
            }
          }
        }
      }
    }


    /// <inheritdoc />
    public RouteProviderEndpoint MapEndpoint(IResolvedRoute route)
    {
      IRouteProvider routeProvider = FindProvider(route.Route.ProviderAlias);
      return routeProvider?.MapEndpoint(route);
    }


    /// <summary>
    /// Call the provider which can resolve the route
    /// </summary>
    async Task<IResolvedRoute> ResolveRouteInternal(IAsyncDocumentSession session, Route route)
    {
      IRouteProvider routeProvider = FindProvider(route.ProviderAlias);
      return await routeProvider?.ResolveRoute(session, route);
    }


    /// <summary>
    /// Find registered route provider for the specified alias
    /// </summary>
    IRouteProvider FindProvider(string alias)
    {
      IRouteProvider routeProvider = Providers.FirstOrDefault(x => x.Alias == alias);

      if (routeProvider == null)
      {
        Logger.LogWarning("Could not locate URL provider {provider}", alias);
      }

      return routeProvider;
    }
  }


  public interface IRoutes
  {
    /// <summary>
    /// Get the URL for an entity
    /// </summary>
    Task<string> GetUrl<T>(T model, object parameters = null);

    /// <summary>
    /// Get the URL for an entity
    /// </summary>
    Task<string> GetUrl<T>(string id, object parameters = null) where T : ZeroIdEntity;

    /// <summary> 
    /// Get the route object for an entity
    /// </summary>
    Task<Route> GetRoute<T>(T model, object parameters = null);

    /// <summary> 
    /// Get the route object for an entity
    /// </summary>
    Task<Route> GetRoute<T>(string id, object parameters = null) where T : ZeroIdEntity;

    /// <summary>
    /// Get URLs for multiple entities
    /// </summary>
    Task<Dictionary<T, string>> GetUrls<T>(IEnumerable<T> models, object parameters = null);

    /// <summary>
    /// Get routes for multiple entities
    /// </summary>
    Task<Dictionary<T, Route>> GetRoutes<T>(IEnumerable<T> models, object parameters = null);

    /// <summary>
    /// Resolve an URL from the specified app and the path
    /// </summary>
    Task<IResolvedRoute> ResolveUrl(Application application, string path);

    /// <summary>
    /// Resolve an URL from an http context
    /// </summary>
    Task<IResolvedRoute> ResolveUrl(HttpContext context);

    /// <summary>
    /// Resolve a route object by passing it to the specified provider
    /// </summary>
    Task<IResolvedRoute> ResolveRoute(Route route);

    /// <summary>
    /// Returns the endpoint which maps 404 requests
    /// </summary>
    NotFoundRoute NotFound(HttpContext context);

    /// <summary>
    /// Purges all routes and rebuilds them by iterating over all registered providers
    /// </summary>
    Task RebuildAllRoutes();

    /// <summary>
    /// Get endpoint the route maps to
    /// </summary>
    RouteProviderEndpoint MapEndpoint(IResolvedRoute route);
  }
}
