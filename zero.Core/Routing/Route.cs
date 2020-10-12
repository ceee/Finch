using System.Collections.Generic;
using zero.Core.Attributes;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  [Collection("Routes")]
  public class Route : IZeroIdEntity, IRoute
  {
    /// <inheritdoc />
    public string Id { get; set; }

    /// <inheritdoc />
    public string AppId { get; set; }

    /// <inheritdoc />
    public string Url { get; set; }

    /// <inheritdoc />
    public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();

    /// <inheritdoc />
    public IList<RouteReference> References { get; set; } = new List<RouteReference>();
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


  public interface IRoute
  {
    /// <summary>
    /// Generated id
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Affected application id
    /// </summary>
    string AppId { get; set; }

    /// <summary>
    /// Generated URL based on the URL provider
    /// </summary>
    string Url { get; set; }

    /// <summary>
    /// Additional parameters
    /// </summary>
    Dictionary<string, object> Params { get; set; }

    /// <summary>
    /// Contains references to the resolved collection entities
    /// </summary>
    IList<RouteReference> References { get; set; }
  }
}
