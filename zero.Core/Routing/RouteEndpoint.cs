namespace zero.Core.Routing
{
  public class RouteEndpoint
  {
    public RouteEndpoint() { }

    public RouteEndpoint(string controller, string action = "Index")
    {
      Type = RouteEndpointType.Controller;
      Controller = controller;
      Action = action;
    }

    public RouteEndpointType Type { get; set; }

    public string Controller { get; set; }

    public string Action { get; set; }
  }


  public enum RouteEndpointType
  {
    Controller = 0
  }
}
