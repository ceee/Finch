using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Security
{
  public class ZeroCookieManager : ChunkingCookieManager, ICookieManager
  {
    protected IZeroOptions Zero { get; set; }


    public ZeroCookieManager(IZeroOptions zero)
    {
      Zero = zero;
    }


    /// <summary>
    /// Explicitly implement this so that we filter the request
    /// </summary>
    string ICookieManager.GetRequestCookie(HttpContext context, string key)
    {
      Uri requestUri = new Uri(context.Request.GetEncodedUrl(), UriKind.RelativeOrAbsolute);
      string path = Zero.BackofficePath.EnsureStartsWith('/').TrimEnd('/');

      if (!context.Request.Path.ToString().StartsWith(path))
      {
        return null;
      }

      return GetRequestCookie(context, key);
    }
  }
}
