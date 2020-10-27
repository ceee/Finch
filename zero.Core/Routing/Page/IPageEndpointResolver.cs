using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public interface IPageEndpointResolver
  {
    /// <summary>
    /// Get MVC endpoint for the page route
    /// </summary>
    RouteProviderEndpoint GetEndpoint(PageRoute route);
  }
}
