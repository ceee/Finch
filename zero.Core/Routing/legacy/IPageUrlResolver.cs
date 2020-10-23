using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public interface IPageUrlResolver
  {
    /// <summary>
    /// Get URL for a page
    /// </summary>
    Task<string> GetUrl(IPage page);

    /// <summary>
    /// Get URL for a page
    /// </summary>
    string GetUrl(IPage page, IEnumerable<IPage> parents);
  }
}
