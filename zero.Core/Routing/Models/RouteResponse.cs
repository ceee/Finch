using Microsoft.AspNetCore.Http;

namespace zero.Routing;

public class RouteResponse
{
  public Route Route { get; set; }

  public HttpContext HttpContext { get; set; }

  public Application App { get; set; }

  public string Path { get; set; }
}
