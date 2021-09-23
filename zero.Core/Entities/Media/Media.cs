using zero.Core.Attributes;

namespace zero.Core.Entities
{
  /// <summary>
  /// A media file (can contain an image or other media like videos and documents)
  /// </summary>
  [Collection("Media")]
  public class Media : ZeroEntity
  {
    /// <summary>
    /// Id/name of the phyiscal folder which is stored on disk/cloud
    /// </summary>
    public string FileId { get; set; }

    /// <summary>
    /// Id of the media folder
    /// </summary>
    public string FolderId { get; set; }

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
    public string Source { get; set; }

    /// <summary>
    /// For images this is the source for a 100x100px thumbnail
    /// </summary>
    public string ThumbnailSource { get; set; }

    /// <summary>
    /// For images this is the source for a [proportional]x210px thumbnail
    /// </summary>
    public string PreviewSource { get; set; }

    /// <summary>
    /// Filesize in bytes
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// Meta data for images
    /// </summary>
    public MediaImageMeta ImageMeta { get; set; }

    /// <summary>
    /// Optional focal point for an image
    /// </summary>
    public MediaFocalPoint FocalPoint { get; set; }

    /// <summary>
    /// Type of the media
    /// </summary>
    public MediaType Type { get; set; }
  }


  public enum MediaSourceSize
  {
    Original = 0,
    Preview = 1,
    Thumbnail = 2
  }
}
