namespace zero.Pages;

/// <summary>
/// A page folder is used to group pages together but should not contain any content
/// </summary>
public sealed class PageFolder : Page
{
  public PageFolder()
  {
    IsActive = true;
  }

  /// <summary>
  /// Whether this folder is included in route building
  /// </summary>
  public bool IsPartOfRoute { get; set; }
}