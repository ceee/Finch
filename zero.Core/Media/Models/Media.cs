namespace zero.Media;

/// <summary>
/// A media file (can contain an image or other media like videos and documents)
/// </summary>
[RavenCollection("Media")]
public class Media : ZeroEntity, ISupportsTrees, IAlwaysActive
{
  public Media()
  {
    IsActive = true;
  }

  /// <summary>
  /// Whether this media item is a folder or a file
  /// </summary>
  public bool IsFolder { get; set; }

  /// <summary>
  /// Id/name of the phyiscal folder which is stored on disk/cloud
  /// </summary>
  public string FileId { get; set; }

  /// <summary>
  /// Id of the parent folder
  /// </summary>
  public string ParentId { get; set; }

  /// <summary>
  /// Additional caption text
  /// </summary>
  public string Caption { get; set; }

  /// <summary>
  /// Path of the media item
  /// </summary>
  public string Path { get; set; }

  /// <summary>
  /// Filesize in bytes
  /// </summary>
  public long Size { get; set; }

  /// <summary>
  /// Meta data for images
  /// </summary>
  public MediaImageMetadata ImageMeta { get; set; }
}