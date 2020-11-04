using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Security
{
  public class ZeroCookieManager : ChunkingCookieManager, ICookieManager
  {
    protected IZeroOptions Zero { get; set; }

    protected bool IsBackoffice { get; private set; }


    public ZeroCookieManager(IZeroOptions zero, bool isBackoffice = false)
    {
      Zero = zero;
      IsBackoffice = isBackoffice;
    }


    /// <summary>
    /// Explicitly implement this so that we filter the request
    /// </summary>
    string ICookieManager.GetRequestCookie(HttpContext context, string key)
    {
      string path = Zero.BackofficePath.EnsureStartsWith('/').TrimEnd('/');
      bool isBackofficeRequest = context.Request.Path.ToString().StartsWith(path);

      if ((IsBackoffice && !isBackofficeRequest) || (!IsBackoffice && isBackofficeRequest))
      {
        return null;
      }

      string cookie = GetRequestCookie(context, key);
      return cookie;
    }
  }
}
