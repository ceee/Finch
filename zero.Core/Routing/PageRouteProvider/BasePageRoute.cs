namespace zero.Routing;

public abstract class BasePageRoute : IRouteModel
{
  public BasePageRoute() { }

  public BasePageRoute(Route route)
  {
    Route = route;
  }

  public Page Page { get; set; }

  public Route Route { get; set; }
}
