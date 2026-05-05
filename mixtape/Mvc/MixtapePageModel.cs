using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Mixtape.Metadata;

namespace Mixtape.Mvc;

public class MixtapePageModel : PageModel
{
  /// <summary>
  /// Get access to the mixtape context for this request
  /// </summary>
  public IMixtapeContext MixtapeContext => _mixtapeContext ??= HttpContext?.RequestServices?.GetService<IMixtapeContext>();
  IMixtapeContext _mixtapeContext;
  
  /// <summary>
  /// Get access to the localizer
  /// </summary>
  public ILocalizer Localizer => _localizer ??= HttpContext?.RequestServices?.GetService<ILocalizer>();
  ILocalizer _localizer;

  /// <summary>
  /// Set metadata for this page
  /// </summary>
  public MetadataOptions Metadata { get; protected set; } = new();

  /// <summary>
  /// Redirect to the current page with the same query string and an optional URL suffix
  /// </summary>
  public IActionResult RedirectToSelf(string suffix = null)
  {
    return Redirect(CurrentUrl(suffix));
  }

  /// <summary>
  /// Get current URL as string with an optional URL suffix
  /// </summary>
  public string CurrentUrl(string suffix = null)
  {
    string rawUrl = Request.GetEncodedPathAndQuery();
    return rawUrl + suffix.Or(string.Empty);
  }
}