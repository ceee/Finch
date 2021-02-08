using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public abstract class BasePageRoute : IResolvedRoute
  {
    public BasePageRoute() { }

    public BasePageRoute(IRoute route)
    {
      Route = route;
    }

    public IPage Page { get; set; }

    public IRoute Route { get; set; }
  }
}
