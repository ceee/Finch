namespace zero.Media;

/// <summary>
/// A media folder contains media and other folders
/// </summary>
[RavenCollection("MediaFolders")]
public class MediaFolder : ZeroEntity, IZeroTreeEntity
{
  /// <summary>
  /// Parent folder id
  /// </summary>
  public string ParentId { get; set; }
}