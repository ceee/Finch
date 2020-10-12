using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class PageUrlResolver : IPageUrlResolver
  {
    const char PATH_SEPARATOR = '/';

    const bool TRAILING_SLASH = false;


    /// <inheritdoc />
    public UrlInfo GetUrl(IApplicationContext context, IPage page, IEnumerable<IPage> parents)
    {
      StringBuilder stringBuilder = new StringBuilder();

      foreach(IPage parent in parents)
      {
        stringBuilder.Append(parent.Alias);
        stringBuilder.Append(PATH_SEPARATOR);
      }

      stringBuilder.Append(page.Alias);
      stringBuilder.Append(PATH_SEPARATOR);

      if (!TRAILING_SLASH)
      {
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
      }

      return new UrlInfo(stringBuilder.ToString(), true, "en-US");
    }
  }
}
