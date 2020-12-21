using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace zero.Core.Routing
{
  public class RequestUrlResolver : IRequestUrlResolver
  {
    protected IHttpContextAccessor HttpContextAccessor { get; private set; }

    protected ILogger<RequestUrlResolver> Logger { get; private set; }

    private static string[] Protocols { get; set; } = new[] { "http://", "https://" };


    public RequestUrlResolver(IHttpContextAccessor httpContextAccessor, ILogger<RequestUrlResolver> logger)
    {
      HttpContextAccessor = httpContextAccessor;
      Logger = logger;
    }


    /// <inheritdoc />
    public string ToAbsolute(string path)
    {
      if (String.IsNullOrWhiteSpace(path))
      {
        return GetRoot();
      }

      if (Protocols.Any(p => path.StartsWith(p, StringComparison.InvariantCultureIgnoreCase)))
      {
        return path;
      }

      path = path.Trim().TrimEnd('/');

      if (path.StartsWith("~/"))
      {
        path = path.Substring(1);
      }
      if (!path.StartsWith("/"))
      {
        return GetRoot(includePath: true) + '/' + path;
      }

      return GetRoot() + path;
    }


    /// <summary>
    /// Get protocol + domain and optional port
    /// </summary>
    protected string GetRoot(bool includePath = false)
    {
      if (!CanResolve())
      {
        Logger.LogWarning("Tried to resolve an URL but no HttpRequest is available");
        return null;
      }

      HttpRequest request = HttpContextAccessor.HttpContext.Request;

      return $"{request.Scheme}://{request.Host.Host}{GetPortUrlPart(request)}{(includePath ? request.PathBase.ToUriComponent() : String.Empty)}";
    }


    /// <summary>
    /// Require a request in order to resolve the URL
    /// </summary>
    protected bool CanResolve() => HttpContextAccessor?.HttpContext?.Request != null;


    /// <summary>
    /// Resolves the port as part of the URL
    /// </summary>
    protected string GetPortUrlPart(HttpRequest request)
    {
      if (!request.Host.Port.HasValue || request.Host.Port == 80 || (request.Host.Port == 443 && request.IsHttps))
      {
        return String.Empty;
      }

      return ":" + request.Host.Port;
    }
  }


  public interface IRequestUrlResolver
  {
    /// <summary>
    /// Converts a relative to an absolute path for the currently used domain
    /// </summary>
    string ToAbsolute(string path);
  }
}
