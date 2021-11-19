using Microsoft.AspNetCore.Http;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class RouteResponse
  {
    public Route Route { get; set; }

    public HttpContext HttpContext { get; set; }

    public Application App { get; set; }

    public string Path { get; set; }
  }
}
