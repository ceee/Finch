using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public abstract class BasePageRoute : IResolvedRoute, IRouteModel
  {
    public BasePageRoute() { }

    public BasePageRoute(Route route)
    {
      Route = route;
    }

    public Page Page { get; set; }

    public Route Route { get; set; }
  }
}
