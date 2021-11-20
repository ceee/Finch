using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace zero.Routing;

public class RequestUrlResolver : IRequestUrlResolver
{
  public IRequestUrlResolver Backoffice { get; private set; }

  protected IHttpContextAccessor HttpContextAccessor { get; private set; }

  protected IZeroOptions Options { get; private set; }

  protected ILogger<RequestUrlResolver> Logger { get; private set; }

  static string[] Protocols = new[] { "http://", "https://", "ftp://", "ftps://", "sftp://", "udp://" };

  bool IsBackoffice = false;


  public RequestUrlResolver(IHttpContextAccessor httpContextAccessor, ILogger<RequestUrlResolver> logger, IZeroOptions options, bool isBackoffice = false)
  {
    HttpContextAccessor = httpContextAccessor;
    Logger = logger;
    Options = options;
    IsBackoffice = isBackoffice;
    if (!isBackoffice)
    {
      Backoffice = new RequestUrlResolver(httpContextAccessor, logger, options, true);
    }
  }


  /// <inheritdoc />
  public string ToAbsolute(string path)
  {
    if (!CanResolve())
    {
      return null;
    }

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


  /// <inheritdoc />
  public string GetRoot(bool includePath = false)
  {
    if (!CanResolve())
    {
      Logger.LogWarning("Tried to resolve an URL but no HttpRequest is available");
      return null;
    }

    HttpRequest request = HttpContextAccessor.HttpContext.Request;

    string suffix = IsBackoffice ? Options.BackofficePath : String.Empty;

    return $"{request.Scheme}://{request.Host.Host}{GetPortUrlPart(request)}{suffix}{(includePath ? request.PathBase.ToUriComponent() : String.Empty)}";
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
  /// URL resolver for backoffice links
  /// </summary>
  IRequestUrlResolver Backoffice { get; }

  /// <summary>
  /// Converts a relative to an absolute path for the currently used domain
  /// </summary>
  string ToAbsolute(string path);

  /// <summary>
  /// Get protocol + domain and optional port
  /// </summary>
  string GetRoot(bool includePath = false);
}
