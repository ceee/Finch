using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class Routes : IRoutes
  {
    protected IDocumentStore Raven { get; set; }
    protected ILogger<Routes> Logger { get; set; }
    protected IEnumerable<IRouteProvider> Providers { get; set; }


    public Routes(IDocumentStore raven, ILogger<Routes> logger, IEnumerable<IRouteProvider> providers)
    {
      Raven = raven;
      Logger = logger;
      Providers = providers;
    }


    /// <inheritdoc />
    public async Task<string> GetUrl<T>(T model) where T : IZeroEntity => await GetUrl(model?.Id);


    /// <inheritdoc />
    public async Task<string> GetUrl(string modelId) => (await GetRoute(modelId))?.Url;


    /// <inheritdoc />
    public async Task<IRoute> GetRoute<T>(T model) where T : IZeroEntity => await GetRoute(model?.Id);


    /// <inheritdoc />
    public async Task<IRoute> GetRoute(string modelId)
    {
      Type type = null; // model.GetType();
      IRouteProvider routeProvider = Providers.FirstOrDefault(x => x.AffectedTypes.Any(t => t.IsAssignableFrom(type)));

      if (routeProvider == null)
      {
        return null;
      }

      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      return await routeProvider.GetRoute(session, null);
    }


    /// <inheritdoc />
    public async Task<IResolvedRoute> ResolveUrl(string appId, string path)
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

      IList<IRoute> routes = await session.Query<IRoute>()
        .Where(x => x.AppId == appId)
        .Where(x => (!x.AllowSuffix && x.Url == path) || (x.AllowSuffix && path.StartsWith(x.Url)))
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
    public async Task<IResolvedRoute> ResolveRoute(IRoute route)
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      return await ResolveRouteInternal(session, route);
    }


    /// <summary>
    /// Call the provider which can resolve the route
    /// </summary>
    async Task<IResolvedRoute> ResolveRouteInternal(IAsyncDocumentSession session, IRoute route)
    {
      IRouteProvider routeProvider = Providers.FirstOrDefault(x => x.Alias == route.ProviderAlias);

      if (routeProvider == null)
      {
        Logger.LogWarning("Could not locate URL provider {provider}", route.ProviderAlias);
        return null;
      }

      return await routeProvider.ResolveRoute(session, route);
    }
  }


  public interface IRoutes
  {
    /// <summary>
    /// Get the URL for an entity
    /// </summary>
    Task<string> GetUrl<T>(T model) where T : IZeroEntity;

    /// <summary>
    /// Get the URL for an entity
    /// </summary>
    Task<string> GetUrl(string modelId);

    /// <summary>
    /// Get the route object for an entity
    /// </summary>
    Task<IRoute> GetRoute<T>(T model) where T : IZeroEntity;

    /// <summary>
    /// Get the route object for an entity
    /// </summary>
    Task<IRoute> GetRoute(string modelId);

    /// <summary>
    /// Resolve an URL from the specified app-id and the path
    /// </summary>
    Task<IResolvedRoute> ResolveUrl(string appId, string path);

    /// <summary>
    /// Resolve a route object by passing it to the specified provider
    /// </summary>
    Task<IResolvedRoute> ResolveRoute(IRoute route);
  }
}
