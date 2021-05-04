using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace zero.Core.Routing
{
  public interface IRouteProvider<T> : IRouteProvider
  {
    /// <summary>
    /// Find URL for an entity
    /// </summary>
    Task<Route> GetRoute(IAsyncDocumentSession session, T model, object parameters = null);

    /// <summary>
    /// Find URLs for mulitple entities
    /// </summary>
    Task<Dictionary<T, Route>> GetRoutes(IAsyncDocumentSession session, IEnumerable<T> models, object parameters = null);

    /// <summary>
    /// Generate unique route ID for a model
    /// </summary>
    string GetRouteId(T model, object parameters = null);
  }


  public interface IRouteProvider
  {
    string Alias { get; }

    Type[] AffectedTypes { get; }

    /// <summary>
    /// Map a route to an MVC endpoint
    /// </summary>
    RouteProviderEndpoint MapEndpoint(IResolvedRoute route);

    /// <summary>
    /// Resolve a route and load optional dependencies into it.
    /// This is the data which is passed to the specified controller action.
    /// </summary>
    Task<IResolvedRoute> ResolveRoute(IAsyncDocumentSession session, Route route);

    /// <summary>
    /// Find URL for an entity
    /// </summary>
    Task<Route> GetRoute(IAsyncDocumentSession session, object model, object parameters = null);

    /// <summary>
    /// Find URLs for mulitple entities
    /// </summary>
    Task<Dictionary<object, Route>> GetRoutes(IAsyncDocumentSession session, IEnumerable<object> models, object parameters = null);

    /// <summary>
    /// Rebuild all routes for this provider so they can be hydrated into the database
    /// </summary>
    Task<IList<Route>> GetAllRoutes(IAsyncDocumentSession session);

    /// <summary>
    /// Generate unique route ID for a model
    /// </summary>
    string GetRouteId(object model, object parameters = null);
  }
}
