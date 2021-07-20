using Microsoft.AspNetCore.Http;

namespace zero.Core.Routing
{
  public class NotFoundRoute : IResolvedRoute
  {
    public NotFoundRoute(HttpContext context)
    {
      Route = new Route()
      {
        Url = context.Request.Path.ToString()
      };
    }

    public Route Route { get; set; }

    public string Controller { get; set; }

    public string Action { get; set; }
  }
}
