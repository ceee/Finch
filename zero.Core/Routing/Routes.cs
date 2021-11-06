using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;


namespace zero.Core.Routing
{
  public class Routes : IRoutes
  {
    public const char PATH_SEPERATOR = '/';

    protected IZeroContext Context { get; set; }
    protected IZeroStore Store { get; set; }
    protected ILogger<Routes> Logger { get; set; }
    protected IEnumerable<IRouteProvider> Providers { get; set; }


    public Routes(IZeroContext context, IZeroStore store, ILogger<Routes> logger, IEnumerable<IRouteProvider> providers)
    {
      Context = context;
      Store = store;
      Logger = logger;
      Providers = providers;
    }


    /// <inheritdoc />
    public async Task<string> GetUrl<T>(T model, object parameters = null) where T : IZeroRouteEntity => (await GetRoute(model, parameters))?.Url;


    /// <inheritdoc />
    public async Task<string> GetUrl<T>(string id, object parameters = null) where T : IZeroRouteEntity => (await GetRoute<T>(id, parameters))?.Url;


    /// <inheritdoc />
    public async Task<Dictionary<T, string>> GetUrls<T>(IEnumerable<T> models, object parameters = null) where T : IZeroRouteEntity => (await GetRoutes(models, parameters)).ToDictionary(x => x.Key, x => x.Value?.Url);


    /// <inheritdoc />
    public async Task<Route> GetRoute<T>(string id, object parameters = null) where T : IZeroRouteEntity
    {
      if (id.IsNullOrEmpty())
      {
        return null;
      }

      Type type = typeof(T);
      IRouteProvider routeProvider = Providers.FirstOrDefault(x => x.CanHandle(type));

      if (routeProvider == null)
      {
        return null;
      }

      RoutingContext context = GetContext();
      T model = await context.Session.LoadAsync<T>(id);

      if (model == null)
      {
        return null;
      }

      return await FindRoute(routeProvider, context, model);
    }


    /// <inheritdoc />
    public async Task<Route> GetRoute<T>(T model, object parameters = null) where T : IZeroRouteEntity
    {
      if (model == null)
      {
        return null;
      }

      if (!TryGetProvider(model, out IRouteProvider routeProvider))
      {
        return null;
      }

      RoutingContext context = GetContext();
      return await FindRoute(routeProvider, context, model);
    }


    /// <inheritdoc />
    public async Task<Dictionary<T, Route>> GetRoutes<T>(IEnumerable<T> models, object parameters = null) where T : IZeroRouteEntity
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

      if (!TryGetProvider(firstExistingModel, out IRouteProvider routeProvider))
      {
        return null;
      }

      RoutingContext context = GetContext();
      Dictionary<string, T> idMap = models.ToDictionary(x => GetId(routeProvider, x), x => x);
      Dictionary<string, Route> routes = await context.Session.LoadAsync<Route>(idMap.Select(x => x.Key));
      Dictionary<T, Route> result = new();

      foreach ((string id, T model) in idMap)
      {
        result.Add(model, routes.GetValueOrDefault(id));
      }

      return result;
    }


    /// <inheritdoc />
    public virtual bool TryGetProvider<T>(T model, out IRouteProvider provider) where T : IZeroRouteEntity
    {
      Type type = model.GetType();
      provider = Providers.FirstOrDefault(x => x.CanHandle(type));
      return provider != null;
    }


    /// <summary>
    /// Map a route to an MVC endpoint
    /// </summary>
    //public virtual RouteProviderEndpoint MapEndpoint(IResolvedRoute route)
    //{
    //  IEnumerable<Func<IResolvedRoute, RouteProviderEndpoint>> resolvers = Options.Routing.EndpointResolvers.GetAll(route.GetType());

    //  foreach (Func<IResolvedRoute, RouteProviderEndpoint> resolver in resolvers.Reverse())
    //  {
    //    RouteProviderEndpoint endpoint = resolver(route);

    //    if (endpoint != null)
    //    {
    //      return endpoint;
    //    }
    //  }

    //  return Options.Routing.DefaultEndpoint;
    //}


    /// <summary>
    /// Find a persisted route for an entity
    /// </summary>
    protected virtual async Task<Route> FindRoute(IRouteProvider provider, RoutingContext context, IZeroRouteEntity model)
    {
      return await provider.Find(context, model);
    }


    /// <summary>
    /// Get ID in collection for an entity 
    /// </summary>
    protected virtual string GetId(IRouteProvider provider, IZeroRouteEntity model)
    {
      string key = provider.Key(model);

      if (key.IsNullOrEmpty())
      {
        return null;
      }

      return "routes." + provider.Alias + "." + key;
    }


    /// <summary>
    /// Build a new routing context
    /// </summary>
    protected virtual RoutingContext GetContext()
    {
      return new(Store, Context, Store.Session());
    }
  }

  public interface IRoutes
  {
    /// <summary>
    /// Get the URL for an entity
    /// </summary>
    Task<string> GetUrl<T>(T model, object parameters = null) where T : IZeroRouteEntity;

    /// <summary>
    /// Get the URL for an entity
    /// </summary>
    Task<string> GetUrl<T>(string id, object parameters = null) where T : IZeroRouteEntity;

    /// <summary>
    /// Get URLs for multiple entities
    /// </summary>
    Task<Dictionary<T, string>> GetUrls<T>(IEnumerable<T> models, object parameters = null) where T : IZeroRouteEntity;

    /// <summary> 
    /// Get the route object for an entity
    /// </summary>
    Task<Route> GetRoute<T>(T model, object parameters = null) where T : IZeroRouteEntity;

    /// <summary> 
    /// Get the route object for an entity
    /// </summary>
    Task<Route> GetRoute<T>(string id, object parameters = null) where T : IZeroRouteEntity;

    /// <summary>
    /// Get routes for multiple entities
    /// </summary>
    Task<Dictionary<T, Route>> GetRoutes<T>(IEnumerable<T> models, object parameters = null) where T : IZeroRouteEntity;

    /// <summary>
    /// Find a provider for a certain entity
    /// </summary>
    bool TryGetProvider<T>(T model, out IRouteProvider provider) where T : IZeroRouteEntity;
  }
}
