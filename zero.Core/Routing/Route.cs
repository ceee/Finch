using System.Collections.Generic;
using zero.Core.Attributes;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  [Collection("Routes")]
  public class Route : ZeroIdEntity
  {
    /// <summary>
    /// Generated URL based on the URL provider
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Alias of the URL provider which generated this route
    /// </summary>
    public string ProviderAlias { get; set; }

    /// <summary>
    /// Enable this property so all routes are catched which start with this Url
    /// </summary>
    public bool AllowSuffix { get; set; }

    /// <summary>
    /// Additional parameters
    /// </summary>
    public Dictionary<string, object> Params { get; set; } = new();

    /// <summary>
    /// Contains references to the resolved collection entities
    /// </summary>
    public List<RouteReference> References { get; set; } = new();

    /// <summary>
    /// Route dependencies can be used for cache busting
    /// </summary>
    public List<string> Dependencies { get; set; } = new();
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
}
