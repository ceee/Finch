namespace zero.Media;

/// <summary>
/// A media folder contains media and other folders
/// </summary>
[RavenCollection("MediaFolders")]
public class MediaFolder : ZeroEntity
{
  /// <summary>
  /// Parent folder id
  /// </summary>
  public string ParentId { get; set; }
}