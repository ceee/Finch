using System;

namespace zero.Core.Entities
{
  /// <summary>
  /// A media file (can contain an image or other media like videos and documents)
  /// </summary>
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
    public int Size { get; set; }

    /// <inheritdoc />
    public MediaDimension Dimension { get; set; }

    /// <inheritdoc />
    public DateTimeOffset LastModifiedDate { get; set; }

    /// <inheritdoc />
    public MediaFocalPoint FocalPoint { get; set; }
  }


  public interface IMedia : IZeroEntity, IAppAwareEntity, IZeroDbConventions
  {
    /// <summary>
    /// Id/name of the folder which is stored on disk/cloud
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
    /// Filesize in bytes
    /// </summary>
    int Size { get; set; }

    /// <summary>
    /// Dimension (width + height) in pixels
    /// </summary>
    MediaDimension Dimension { get; set; }

    /// <summary>
    /// Time the file has last changed
    /// </summary>
    DateTimeOffset LastModifiedDate { get; set; }

    /// <summary>
    /// Optional focal point for an image
    /// </summary>
    MediaFocalPoint FocalPoint { get; set; }
  }
}
