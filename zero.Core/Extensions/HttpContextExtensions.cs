using Microsoft.AspNetCore.Http;

namespace zero.Core.Extensions
{
  internal static class HttpContextExtensions
  {
    /// <summary>
    /// Whether the current request is a backoffice request
    /// </summary>
    public static bool IsBackofficeRequest(this HttpContext context, string backofficePath)
    {
      string path = backofficePath.EnsureStartsWith('/').TrimEnd('/');
      return context.Request.Path.ToString().StartsWith(path);
    }
  }
}
