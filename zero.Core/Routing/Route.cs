using System.Collections.Generic;
using zero.Core.Attributes;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  [Collection("Routes")]
  public class Route : IRoute
  {
    /// <inheritdoc />
    public string Id { get; set; }

    /// <inheritdoc />
    public string Url { get; set; }

    /// <inheritdoc />
    public string ProviderAlias { get; set; }

    /// <inheritdoc />
    public bool AllowSuffix { get; set; }

    /// <inheritdoc />
    public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();

    /// <inheritdoc />
    public IList<RouteReference> References { get; set; } = new List<RouteReference>();

    /// <inheritdoc />
    public IList<string> Dependencies { get; set; } = new List<string>();
  }


  public class RouteReference
  {
    public string Id { get; set; }

    public string Collection { get; set; }

    public RouteReference() { }

    public RouteReference(string id, string collection)
    {
      Id = id;
      Collection = collection;
    }
  }


  [Collection("Routes")]
  public interface IRoute : IZeroIdEntity, IZeroDbConventions
  {
    /// <summary>
    /// Generated URL based on the URL provider
    /// </summary>
    string Url { get; set; }

    /// <summary>
    /// Alias of the URL provider which generated this route
    /// </summary>
    string ProviderAlias { get; set; }

    /// <summary>
    /// Enable this property so all routes are catched which start with this Url
    /// </summary>
    public bool AllowSuffix { get; set; }

    /// <summary>
    /// Additional parameters
    /// </summary>
    Dictionary<string, object> Params { get; set; }

    /// <summary>
    /// Contains references to the resolved collection entities
    /// </summary>
    IList<RouteReference> References { get; set; }

    /// <summary>
    /// Route dependencies can be used for cache busting
    /// </summary>
    IList<string> Dependencies { get; set; }
  }
}
