using Microsoft.AspNetCore.Http;

namespace Finch.Extensions;

public static class HttpContextExtensions
{
  /// <summary>
  /// Whether the current request is an AJAX request
  /// </summary>
  public static bool IsAjaxRequest(this HttpContext context)
  {
    if (context?.Request?.Headers == null)
    {
      return false;
    }

    return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
  }


  /// <summary>
  /// Whether the current request only accepts application/json
  /// </summary>
  public static bool IsJsonRequest(this HttpContext context)
  {
    if (context?.Request?.Headers == null)
    {
      return false;
    }

    return context.Request.Headers["Accept"] == "application/json";
  }


  /// <summary>
  /// Get IP Address of the client
  /// </summary>
  public static string GetClientIpAddress(this HttpContext context)
  {
    return context.Connection.RemoteIpAddress?.ToString();
    //string ipAddress = context.GetServerVariable("HTTP_X_FORWARDED_FOR");
    //return !ipAddress.IsNullOrEmpty() ? ipAddress.Split(',')[0] : context.GetServerVariable("REMOTE_ADDR");
  }
  
  
  public static bool IsPartOfUrl(this HttpContext model, string url)
  {
    if (url.IsNullOrEmpty())
    {
      return false;
    }

    string destination = url.EnsureStartsWith('/').TrimEnd('/');
    string current = model.Request.Path.ToUriComponent().TrimEnd('/');
    return current.StartsWith(destination, StringComparison.InvariantCultureIgnoreCase);
  }


  public static bool IsUrl(this HttpContext model, string url)
  {
    if (url.IsNullOrEmpty())
    {
      return false;
    }

    string destination = url.EnsureStartsWith('/').TrimEnd('/');
    string current = model.Request.Path.ToUriComponent().TrimEnd('/');
    return current.Equals(destination, StringComparison.InvariantCultureIgnoreCase);
  }
}
