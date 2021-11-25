namespace zero.Media;

/// <summary>
/// A media file (can contain an image or other media like videos and documents)
/// </summary>
[RavenCollection("Media")]
public class Media : ZeroEntity, IZeroTreeEntity
{
  /// <summary>
  /// Id/name of the phyiscal folder which is stored on disk/cloud
  /// </summary>
  public string FileId { get; set; }

  /// <summary>
  /// Id of the parent folder
  /// </summary>
  public string ParentId { get; set; }

  /// <summary>
  /// Alternative text which is used when the image can't be loaded
  /// </summary>
  public string AlternativeText { get; set; }

  /// <summary>
  /// Additional caption text
  /// </summary>
  public string Caption { get; set; }

  /// <summary>
  /// Path of the media item
  /// </summary>
  public string Path { get; set; }

  /// <summary>
  /// Define custom thumbnails which are generated on upload 
  /// (see IZeroOptions.For<MediaOption>().Thumbnails)
  /// </summary>
  public Dictionary<string, string> Thumbnails { get; set; } = new();

  /// <summary>
  /// Filesize in bytes
  /// </summary>
  public long Size { get; set; }

  /// <summary>
  /// Meta data for images
  /// </summary>
  public MediaImageMetadata ImageMeta { get; set; }

  /// <summary>
  /// Optional focal point for an image
  /// </summary>
  public MediaFocalPoint FocalPoint { get; set; }

  /// <summary>
  /// Type of the media
  /// </summary>
  public MediaType Type { get; set; }
}