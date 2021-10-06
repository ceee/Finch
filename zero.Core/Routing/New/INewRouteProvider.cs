using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public interface INewRouteProvider<T> where T : IZeroRouteEntity
  {
    string Alias { get; }

    /// <summary>
    /// Warmup is only called once.
    /// </summary>
    Task Warmup();

    /// <summary>
    /// Generate unique route ID for a model
    /// </summary>
    string Id(T model);

    /// <summary>
    /// Build URL for a model
    /// </summary>
    string Url(T model);

    /// <summary>
    /// Create route entity from a model
    /// </summary>
    Route Create(T model);

    /// <summary>
    /// 
    /// </summary>
    //bool HasChanged(T newModel, T oldModel);

    /// <summary>
    /// Get all models which should be provided and handled by this instance
    /// </summary>
    Task<IEnumerable<T>> All();

    /// <summary>
    /// Build a resolved route from a route response
    /// </summary>
    Task<IResolvedRoute> Resolve(RouteResponse response);
  }
}
