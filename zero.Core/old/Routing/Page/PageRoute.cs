using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class PageRoute : BasePageRoute
  {
    public PageRoute() { }
    public PageRoute(Route route) : base(route) { }

    public IList<Page> Parents { get; set; }

    public string PageType { get; set; }
  }
}
