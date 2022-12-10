namespace zero.Media;

/// <summary>
/// A media file (can contain an image or other media like videos and documents)
/// </summary>
public class Media : ZeroEntity, ISupportsTrees
{
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
  /// Path of the media item
  /// </summary>
  public string Path { get; set; }

  /// <summary>
  /// Filesize in bytes
  /// </summary>
  public long Size { get; set; }
  
  /// <summary>
  /// Alternative text which is used when the media can't be loaded
  /// </summary>
  public string AltText { get; set; }
  
  /// <summary>
  /// Additional caption text
  /// </summary>
  public string Caption { get; set; }

  /// <summary>
  /// Meta data for images/videos
  /// </summary>
  public MediaMetadata Metadata { get; set; }
  
  /// <summary>
  /// Define custom thumbnails which are generated on upload 
  /// (see IZeroOptions.For<MediaOptions>().Thumbnails)
  /// </summary>
  public Dictionary<string, string> Thumbnails { get; set; } = new();
}