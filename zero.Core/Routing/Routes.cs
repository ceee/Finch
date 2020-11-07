using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Routing
{
  public class Routes : IRoutes
  {
    protected IDocumentStore Raven { get; set; }
    protected ILogger<Routes> Logger { get; set; }
    protected IEnumerable<IRouteProvider> Providers { get; set; }
    protected IApplicationContext Context { get; set; }


    public Routes(IDocumentStore raven, ILogger<Routes> logger, IEnumerable<IRouteProvider> providers, IApplicationContext context)
    {
      Raven = raven;
      Logger = logger;
      Providers = providers;
      Context = context;
    }


    /// <inheritdoc />
    public async Task<string> GetUrl<T>(T model) => (await GetRoute(model))?.Url;


    /// <inheritdoc />
    public async Task<Dictionary<T, string>> GetUrls<T>(params T[] models) => (await GetRoutes(models)).ToDictionary(x => x.Key, x => x.Value?.Url);


    /// <inheritdoc />
    public async Task<IRoute> GetRoute<T>(T model)
    {
      Type type = model.GetType();
      IRouteProvider routeProvider = Providers.FirstOrDefault(x => x.AffectedTypes.Any(t => t.IsAssignableFrom(type)));

      if (routeProvider == null)
      {
        return null;
      }

      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      return await routeProvider.GetRoute(session, model);
    }


    /// <inheritdoc />
    public async Task<Dictionary<T, IRoute>> GetRoutes<T>(params T[] models)
    {
      if (models.Length < 1)
      {
        return new();
      }

      Type type = models[0].GetType();
      IRouteProvider routeProvider = Providers.FirstOrDefault(x => x.AffectedTypes.Any(t => t.IsAssignableFrom(type)));

      if (routeProvider == null)
      {
        return null;
      }

      Dictionary<T, IRoute> result = new Dictionary<T, IRoute>();

      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

      foreach (T model in models)
      {
        result.TryAdd(model, await routeProvider.GetRoute(session, model));
      }

      return result;
    }


    /// <inheritdoc />
    public async Task<IResolvedRoute> ResolveUrl(string appId, string path)
    {
      path = path.Length > 1 ? path.TrimEnd('/') : path;

      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

      string[] pathParts = path.Trim('/').Split('/');
      string[] parts = new string[pathParts.Length];

      int min = parts.Length;
      foreach (string pathPart in pathParts)
      {
        for (int i = 0; i < min; i++)
        {
          parts[i] += '/' + pathPart;
        }
        min -= 1;
      }

      IList<IRoute> routes = await session.Query<IRoute>()
        .Where(x => x.AppId == appId)
        .Where(x => (!x.AllowSuffix && x.Url == path) || (x.AllowSuffix && x.Url.In(parts)))
        .Include("References[].Id")
        .Include("Dependencies")
        .ToListAsync();

      if (routes.Count > 1)
      {
        Logger.LogWarning("Multiple routes {routes} were found for {path}", routes.Select(x => x.Id), path);
      }
      else if (routes.Count < 1)
      {
        return null;
      }

      return await ResolveRouteInternal(session, routes.First());
    }


    /// <inheritdoc />
    public async Task<IResolvedRoute> ResolveUrl(HttpContext context)
    {
      IApplication app = await Context.ResolveFromRequest(context);
      string path = context.Request.Path;

      if (app == null)
      {
        return null;
      }

      return await ResolveUrl(app.Id, path);
    }


    /// <inheritdoc />
    public async Task<IResolvedRoute> ResolveRoute(IRoute route)
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      return await ResolveRouteInternal(session, route);
    }


    /// <inheritdoc />
    public async Task<IList<IRoute>> RebuildAllRoutes()
    {
      int count = 0;
      List<IRoute> all = new List<IRoute>();
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      session.Advanced.MaxNumberOfRequestsPerSession = 1000;

      foreach (IRouteProvider provider in Providers)
      {
        // get all routes for this provider
        IList<IRoute> routes = await provider.GetAllRoutes(session);

        // delete all registered routes in the database for this provider
        await Raven.PurgeAsync<IRoute>($"where {nameof(IRoute.ProviderAlias)} = $alias", new Raven.Client.Parameters()
        {
          { "alias", provider.Alias }
        });

        // store new routes
        using (BulkInsertOperation bulkInsert = Raven.BulkInsert())
        {
          foreach (IRoute route in routes)
          {
            await bulkInsert.StoreAsync(route, route.Id);
            count += 1;
          }
        }

        all.AddRange(routes);
      }

      return all;
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
    async Task<IResolvedRoute> ResolveRouteInternal(IAsyncDocumentSession session, IRoute route)
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
    Task<string> GetUrl<T>(T model);

    /// <summary> 
    /// Get the route object for an entity
    /// </summary>
    Task<IRoute> GetRoute<T>(T model);

    /// <summary>
    /// Get URLs for multiple entities
    /// </summary>
    Task<Dictionary<T, string>> GetUrls<T>(params T[] models);

    /// <summary>
    /// Get routes for multiple entities
    /// </summary>
    Task<Dictionary<T, IRoute>> GetRoutes<T>(params T[] models);

    /// <summary>
    /// Resolve an URL from the specified app-id and the path
    /// </summary>
    Task<IResolvedRoute> ResolveUrl(string appId, string path);

    /// <summary>
    /// Resolve an URL from an http context
    /// </summary>
    Task<IResolvedRoute> ResolveUrl(HttpContext context);

    /// <summary>
    /// Resolve a route object by passing it to the specified provider
    /// </summary>
    Task<IResolvedRoute> ResolveRoute(IRoute route);

    /// <summary>
    /// Purges all routes and rebuilds them by iterating over all registered providers
    /// </summary>
    Task<IList<IRoute>> RebuildAllRoutes();

    /// <summary>
    /// Get endpoint the route maps to
    /// </summary>
    RouteProviderEndpoint MapEndpoint(IResolvedRoute route);
  }
}
