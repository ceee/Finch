namespace zero.Routing;

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


  public RouteEndpoint NotFoundEndpoint { get; set; }


  public RouteEndpoint DefaultEndpoint { get; set; }

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