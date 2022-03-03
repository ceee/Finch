namespace zero.Preview;

public class PreviewRoute : IRouteModel
{
  public Route Route { get; set; }

  public IRouteModel PreviewedRouteModel { get; set; }
}