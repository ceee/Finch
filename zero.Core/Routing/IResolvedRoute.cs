using System;
using System.Collections.Generic;
using System.Text;

namespace zero.Core.Routing
{
  public interface IResolvedRoute
  {
    IRoute Route { get; set; }
  }
}
