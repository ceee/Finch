using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;

namespace zero;

public class ContextualCookieManager : ChunkingCookieManager, ICookieManager
{
  protected Func<HttpContext, string, bool> Validate { get; private set; } = (ctx, key) => true;


  public ContextualCookieManager(Func<HttpContext, string, bool> onValidate)
  {
    Validate = onValidate;
  }


  /// <summary>
  /// Explicitly implement this so that we filter the request
  /// </summary>
  string ICookieManager.GetRequestCookie(HttpContext context, string key)
  {
    return !Validate(context, key) ? null : GetRequestCookie(context, key);
  }
}