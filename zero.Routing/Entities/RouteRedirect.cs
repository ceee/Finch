using zero.Core.Entities;

namespace zero.Routing;

[RavenCollection("RouteRedirects")]
public class RouteRedirect : ZeroEntity
{
  /// <summary>
  /// Redirect type
  /// </summary>
  public RouteRedirectType RedirectType { get; set; }

  /// <summary>
  /// URL which is redirected
  /// </summary>
  public string SourceUrl { get; set; }

  /// <summary>
  /// Destination URL for automated (non-custom) redirects
  /// </summary>
  public string TargetUrl { get; set; }

  /// <summary>
  /// Target link
  /// </summary>
  public Link Target { get; set; }

  /// <summary>
  /// Whether this redirect is automated (from route interceptor updates) or custom-created.
  /// non-custom redirects are automatically populated and updated.
  /// </summary>
  public bool IsAutomated { get; set; }
}


public enum RouteRedirectType
{
  Permanent = 301,
  Temporary = 302
}