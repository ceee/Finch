namespace zero.Backoffice.Sections;

/// <summary>
/// Global list entities
/// </summary>
public class SpacesSection : IInternalSection
{
  /// <inheritdoc />
  public string Alias => Constants.Sections.Spaces;

  /// <inheritdoc />
  public string Name => "@sections.item.spaces";

  /// <inheritdoc />
  public string Icon => "fth-layers";

  /// <inheritdoc />
  public string Color => "#f9c202";

  /// <inheritdoc />
  public IList<IChildSection> Children => new List<IChildSection>();
}