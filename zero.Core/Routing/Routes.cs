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

    protected IZeroDocumentSession Session { get; set; }
    protected ILogger<Routes> Logger { get; set; }
    protected IEnumerable<IRouteProvider> Providers { get; set; }


    public Routes(IZeroDocumentSession session, ILogger<Routes> logger, IEnumerable<IRouteProvider> providers)
    {
      Session = session;
      Logger = logger;
      Providers = providers;
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

      T model = await Session.LoadAsync<T>(id);

      if (model == null)
      {
        return null;
      }

      return await routeProvider.GetRoute(Session, model, parameters);
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

      return await routeProvider.GetRoute(Session, model, parameters);
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

      return (await routeProvider.GetRoutes(Session, models.Select(x => (object)x), parameters)).ToDictionary(x => (T)x.Key, x => x.Value);
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
    /// Get URLs for multiple entities
    /// </summary>
    Task<Dictionary<T, string>> GetUrls<T>(IEnumerable<T> models, object parameters = null);

    /// <summary> 
    /// Get the route object for an entity
    /// </summary>
    Task<Route> GetRoute<T>(T model, object parameters = null);

    /// <summary> 
    /// Get the route object for an entity
    /// </summary>
    Task<Route> GetRoute<T>(string id, object parameters = null) where T : ZeroIdEntity;

    /// <summary>
    /// Get routes for multiple entities
    /// </summary>
    Task<Dictionary<T, Route>> GetRoutes<T>(IEnumerable<T> models, object parameters = null);
  }
}
