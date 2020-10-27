using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class PageRoute : IResolvedRoute
  {
    public PageRoute(IRoute route)
    {
      Route = route;
    }

    public IPage Page { get; set; }

    public IList<IPage> Parents { get; set; }

    public IRoute Route { get; set; }
  }
}
