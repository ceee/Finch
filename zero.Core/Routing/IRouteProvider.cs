using Raven.Client.Documents.Session;
using System;
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
    /// Generate non-random route ID for a model
    /// </summary>
    string GetRouteId(T model);
  }


  public interface IRouteProvider
  {
    string Alias { get; }

    Type[] AffectedTypes { get; }

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
    /// Generate non-random route ID for a model
    /// </summary>
    string GetRouteId(object model);
  }
}
