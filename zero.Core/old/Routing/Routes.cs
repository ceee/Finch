using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;


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
    public async Task<string> GetUrl<T>(T model) where T : IZeroRouteEntity => (await GetRoute(model))?.Url;


    /// <inheritdoc />
    public async Task<string> GetUrl<T>(string id) where T : IZeroRouteEntity => (await GetRoute<T>(id))?.Url;


    /// <inheritdoc />
    public async Task<Dictionary<T, string>> GetUrls<T>(params T[] models) where T : IZeroRouteEntity => (await GetRoutes(models)).ToDictionary(x => x.Key, x => x.Value?.Url);


    /// <inheritdoc />
    public async Task<Route> GetRoute<T>(string id) where T : IZeroRouteEntity
    {
      T model = await GetContext().Session.LoadAsync<T>(id);
      return await GetRoute(model);
    }


    /// <inheritdoc />
    public async Task<Route> GetRoute<T>(T model) where T : IZeroRouteEntity
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
    public async Task<Dictionary<T, Route>> GetRoutes<T>(params T[] models) where T : IZeroRouteEntity
    {
      if (models.Length < 1)
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
      return (await routeProvider.Find(context, models.Select(x => (IZeroRouteEntity)x).ToArray())).ToDictionary(x => (T)x.Key, x => x.Value);
    }


    /// <inheritdoc />
    public virtual bool TryGetProvider<T>(T model, out IRouteProvider provider) where T : IZeroRouteEntity
    {
      Type type = model.GetType();
      provider = Providers.OrderByDescending(x => x.Priority).FirstOrDefault(x => x.CanHandle(type));
      return provider != null;
    }


    /// <inheritdoc />
    public virtual bool TryGetProvider(string alias, out IRouteProvider provider)
    {
      provider = Providers.OrderByDescending(x => x.Priority).FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
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
    Task<string> GetUrl<T>(T model) where T : IZeroRouteEntity;

    /// <summary>
    /// Get the URL for an entity
    /// </summary>
    Task<string> GetUrl<T>(string id) where T : IZeroRouteEntity;

    /// <summary>
    /// Get URLs for multiple entities
    /// </summary>
    Task<Dictionary<T, string>> GetUrls<T>(params T[] models) where T : IZeroRouteEntity;

    /// <summary> 
    /// Get the route object for an entity
    /// </summary>
    Task<Route> GetRoute<T>(T model) where T : IZeroRouteEntity;

    /// <summary> 
    /// Get the route object for an entity
    /// </summary>
    Task<Route> GetRoute<T>(string id) where T : IZeroRouteEntity;

    /// <summary>
    /// Get routes for multiple entities
    /// </summary>
    Task<Dictionary<T, Route>> GetRoutes<T>(params T[] models) where T : IZeroRouteEntity;

    /// <summary>
    /// Find a provider for a certain entity
    /// </summary>
    bool TryGetProvider<T>(T model, out IRouteProvider provider) where T : IZeroRouteEntity;

    /// <summary>
    /// Find a provider by alias
    /// </summary>
    bool TryGetProvider(string alias, out IRouteProvider provider);
  }
}
