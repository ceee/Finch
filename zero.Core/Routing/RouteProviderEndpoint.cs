namespace zero.Core.Routing
{
  public class RouteProviderEndpoint
  {
    public RouteProviderEndpoint() { }

    public RouteProviderEndpoint(string controller, string action = "Index")
    {
      Controller = controller;
      Action = action;
    }

    public string Controller { get; set; }

    public string Action { get; set; }
  }
}
