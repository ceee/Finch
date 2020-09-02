using System;
using zero.Core.Attributes;

namespace zero.Core.Entities
{
  /// <inheritdoc />
  public class Media : ZeroEntity, IMedia
  {
    /// <inheritdoc />
    public string AppId { get; set; }

    /// <inheritdoc />
    public string FileId { get; set; }

    /// <inheritdoc />
    public string FolderId { get; set; }

    /// <inheritdoc />
    public string AlternativeText { get; set; }

    /// <inheritdoc />
    public string Caption { get; set; }

    /// <inheritdoc />
    public string Source { get; set; }

    /// <inheritdoc />
    public string ThumbnailSource { get; set; }

    /// <inheritdoc />
    public string PreviewSource { get; set; }

    /// <inheritdoc />
    public long Size { get; set; }

    /// <inheritdoc />
    public MediaImageMeta ImageMeta { get; set; }

    /// <inheritdoc />
    public DateTimeOffset LastModifiedDate { get; set; }

    /// <inheritdoc />
    public MediaFocalPoint FocalPoint { get; set; }

    /// <inheritdoc />
    public MediaType Type { get; set; }
  }


  /// <summary>
  /// A media file (can contain an image or other media like videos and documents)
  /// </summary>
  [Collection("Media")]
  public interface IMedia : IZeroEntity, IAppAwareEntity, IZeroDbConventions
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
    string AlternativeText { get; set; }

    /// <summary>
    /// Additional caption text
    /// </summary>
    string Caption { get; set; }

    /// <summary>
    /// Path of the media item
    /// </summary>
    string Source { get; set; }

    /// <summary>
    /// For images this is the source for a 100x100px thumbnail
    /// </summary>
    string ThumbnailSource { get; set; }

    /// <summary>
    /// For images this is the source for a [proportional]x210px thumbnail
    /// </summary>
    string PreviewSource { get; set; }

    /// <summary>
    /// Filesize in bytes
    /// </summary>
    long Size { get; set; }

    /// <summary>
    /// Meta data for images
    /// </summary>
    MediaImageMeta ImageMeta { get; set; }

    /// <summary>
    /// Optional focal point for an image
    /// </summary>
    MediaFocalPoint FocalPoint { get; set; }

    /// <summary>
    /// Type of the media
    /// </summary>
    MediaType Type { get; set; }
  }
}
