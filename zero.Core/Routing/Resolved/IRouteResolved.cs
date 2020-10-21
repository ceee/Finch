using System;
using System.Collections.Generic;
using System.Text;

namespace zero.Core.Routing
{
  public interface IRouteResolved
  {
    IRoute Route { get; set; }
  }
}
