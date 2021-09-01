using Microsoft.AspNetCore.Http;

namespace zero.Core.Extensions
{
  public static class HttpContextExtensions
  {
    /// <summary>
    /// Whether the current request is a backoffice request
    /// </summary>
    public static bool IsBackofficeRequest(this HttpContext context, string backofficePath)
    {
      string path = backofficePath.EnsureStartsWith('/').TrimEnd('/');
      return context.Request.Path.ToString().StartsWith(path);
    }


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
  }
}
