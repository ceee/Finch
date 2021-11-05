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
      PageRouteIdBuilder = new PageRouteIdBuilder();
      DefaultEndpoint = new("ZeroFrontend", "Index");
      EndpointResolvers = new();
      PageResolvers = new();
      //ErrorReexecutionPath = "/error";
      NotFoundEndpoint = null;
    }


    public IPageRouteIdBuilder PageRouteIdBuilder { get; set; }


    public RouteProviderEndpoint NotFoundEndpoint { get; set; }


    public RouteProviderEndpoint DefaultEndpoint { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public RoutingEndpointOptions EndpointResolvers { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public RoutingPageResolverOptions PageResolvers { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ErrorReexecutionPath { get; set; }
  }
}
