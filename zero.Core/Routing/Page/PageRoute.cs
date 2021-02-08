using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class PageRoute : BasePageRoute
  {
    public PageRoute() { }
    public PageRoute(IRoute route) : base(route) { }

    public IList<IPage> Parents { get; set; }
  }
}
