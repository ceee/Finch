using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public interface IPageUrlResolver
  {
    /// <summary>
    /// Get URL for a page
    /// </summary>
    UrlInfo GetUrl(IApplicationContext context, IPage page, IEnumerable<IPage> parents);
  }
}
