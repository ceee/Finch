namespace zero.Backoffice.Sections;

/// <summary>
/// Media items (images, videos, documents) grouped in folders
/// </summary>
public class MediaSection : IInternalBackofficeSection
{
  /// <inheritdoc />
  public string Alias => Constants.Sections.Media;

  /// <inheritdoc />
  public string Name => "@sections.item.media";

  /// <inheritdoc />
  public string Icon => "fth-image";

  /// <inheritdoc />
  public string Color => "#d82853";

  /// <inheritdoc />
  public IList<IChildBackofficeSection> Children => new List<IChildBackofficeSection>();
}