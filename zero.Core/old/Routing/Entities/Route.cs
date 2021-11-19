using System;
using System.Collections.Generic;
using zero.Core.Attributes;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  [Collection("Routes")]
  public class Route : ZeroIdEntity
  {
    public Route() { }

    public Route(string providerAlias)
    {
      ProviderAlias = providerAlias;
    }

    public Route(string id, string url, string providerAlias)
    {
      Id = id;
      Url = url;
      ProviderAlias = providerAlias;
    }

    /// <summary>
    /// Id of the referenced entity
    /// </summary>
    public string ReferenceId { get; set; }

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
    /// Route dependencies can be used for cache busting
    /// </summary>
    public List<string> Dependencies { get; set; } = new();


    /// <summary>
    /// Update an existing route with regenerated data
    /// </summary>
    public virtual Route Update(Route newRoute)
    {
      ReferenceId = newRoute.ReferenceId;
      Url = newRoute.Url;
      AllowSuffix = newRoute.AllowSuffix;
      Params = newRoute.Params;
      Dependencies = newRoute.Dependencies;
      return this;
    }


    /// <summary>
    /// Create a new route
    /// </summary>
    public static Route Create(string id, string providerAlias)
    {
      return new(providerAlias) { Id = id };
    }
  }
}
