namespace zero.Routing;

public interface IRouteModel
{
  Route Route { get; set; }
}

public class RouteModel : IRouteModel
{
  public Route Route { get; set; }
}
