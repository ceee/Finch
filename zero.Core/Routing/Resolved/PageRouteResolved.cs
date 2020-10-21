using System;
using System.Collections.Generic;
using System.Text;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class PageRouteResolved : IRouteResolved
  {
    public PageRouteResolved() { }

    public PageRouteResolved(IRoute route)
    {
      Route = route;
    }

    public IPage Page { get; set; }

    public IList<IPage> Parents { get; set; }

    public IRoute Route { get; set; }
  }
}
