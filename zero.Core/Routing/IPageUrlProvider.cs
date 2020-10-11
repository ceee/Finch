using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public interface IPageUrlProvider
  {
    /// <summary>
    /// Get URL for a page
    /// </summary>
    UrlInfo GetUrl(ApplicationContext context, IPage page, IEnumerable<IPage> parents);
  }
}
