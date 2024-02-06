using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using zero.Metadata;

namespace zero.Mvc;

public abstract class ZeroPageModel : PageModel
{
  /// <summary>
  /// Get access to the zero context for this request
  /// </summary>
  public IZeroContext ZeroContext => _zeroContext ?? (_zeroContext = HttpContext?.RequestServices?.GetService<IZeroContext>());
  IZeroContext _zeroContext;
  
  /// <summary>
  /// Get access to the localizer
  /// </summary>
  public ILocalizer Localizer => _localizer ?? (_localizer = HttpContext?.RequestServices?.GetService<ILocalizer>());
  ILocalizer _localizer;

  /// <summary>
  /// Set metadata for this page
  /// </summary>
  public MetadataOptions Metadata { get; protected set; } = new();
}