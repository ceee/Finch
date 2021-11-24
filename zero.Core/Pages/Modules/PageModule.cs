namespace zero.Pages;

/// <summary>
/// A module can consist of unlimited properties and be rendered as you wish
/// The backoffice rendering is done by an IRenderer
/// </summary>
public class PageModule : ZeroIdEntity
{
  /// <summary>
  /// Sort order
  /// </summary>
  public uint Sort { get; set; }

  /// <summary>
  /// Whether the module is visible in the frontend
  /// </summary>
  public bool IsActive { get; set; }

  /// <summary>
  /// Alias of the used module type
  /// </summary>
  public string ModuleTypeAlias { get; set; }
}