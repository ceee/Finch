namespace zero.Backoffice.Sections;

/// <summary>
/// Manage the page tree in this section
/// </summary>
public class PagesSection : IInternalSection
{
  /// <inheritdoc />
  public string Alias => Constants.Sections.Pages;

  /// <inheritdoc />
  public string Name => "@sections.item.pages";

  /// <inheritdoc />
  public string Icon => "fth-folder";

  /// <inheritdoc />
  public string Color => "#0cb0f5";

  /// <inheritdoc />
  public IList<IChildSection> Children => new List<IChildSection>();
}