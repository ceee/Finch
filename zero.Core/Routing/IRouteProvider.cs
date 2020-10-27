using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace zero.Core.Routing
{
  public interface IRouteProvider<in T> : IRouteProvider
  {
    /// <summary>
    /// Find URL for an entity
    /// </summary>
    Task<IRoute> GetRoute(IAsyncDocumentSession session, T model);

    /// <summary>
    /// Generate unique route ID for a model
    /// </summary>
    string GetRouteId(T model);
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
    Task<IResolvedRoute> ResolveRoute(IAsyncDocumentSession session, IRoute route);

    /// <summary>
    /// Find URL for an entity
    /// </summary>
    Task<IRoute> GetRoute(IAsyncDocumentSession session, object model);

    /// <summary>
    /// Rebuild all routes for this provider so they can be hydrated into the database
    /// </summary>
    Task<IList<IRoute>> GetAllRoutes(IAsyncDocumentSession session);

    /// <summary>
    /// Generate unique route ID for a model
    /// </summary>
    string GetRouteId(object model);
  }
}
