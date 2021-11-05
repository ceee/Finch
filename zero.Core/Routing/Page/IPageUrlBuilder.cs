using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public interface IPageUrlBuilder
  {
    /// <summary>
    /// Get URL for a page
    /// </summary>
    string GetUrl(Page page, IEnumerable<Page> parents);

    /// <summary>
    /// Get the part of the URL (by querying UrlAlias and Alias) for this page
    /// </summary>
    string GetUrlPart(Page page);
  }
}
