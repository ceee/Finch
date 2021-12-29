namespace zero.Backoffice.UIComposition;

/// <summary>
/// Global list entities
/// </summary>
public class SpacesSection : IInternalBackofficeSection
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
  public int Sort => 200;

  /// <inheritdoc />
  public IList<IChildBackofficeSection> Children => new List<IChildBackofficeSection>();
}