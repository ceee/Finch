using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Routing;

namespace zero.Core.Options
{
  public class RoutingOptions
  {
    public RoutingOptions()
    {
      DefaultEndpoint = new("ZeroFrontend", "Index");
      EndpointResolvers = new();
      PageResolvers = new();
    }


    public RouteProviderEndpoint DefaultEndpoint { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public RoutingEndpointOptions EndpointResolvers { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public RoutingPageResolverOptions PageResolvers { get; set; }
  }
}
