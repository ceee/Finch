using System;

namespace zero.Core.Entities
{
  /// <summary>
  /// A media file (can contain an image or other media like videos and documents)
  /// </summary>
  public class Media : IMedia
  {
    /// <inheritdoc />
    public string Id { get; set; }

    /// <inheritdoc />
    public string Name { get; set; }

    /// <inheritdoc />
    public string AlternativeText { get; set; }

    /// <inheritdoc />
    public string Caption { get; set; }

    /// <inheritdoc />
    public string Source { get; set; }

    /// <inheritdoc />
    public bool HasThumbnail { get; set; }

    /// <inheritdoc />
    public int Size { get; set; }

    /// <inheritdoc />
    public MediaDimension Dimension { get; set; }

    /// <inheritdoc />
    public DateTimeOffset LastModifiedDate { get; set; }

    /// <inheritdoc />
    public MediaFocalPoint FocalPoint { get; set; }
  }


  public interface IMedia : IZeroEntity
  {
    /// <summary>
    /// File name
    /// </summary>
    public string Name { get; set; }

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
    /// Whether this file has a thumbnail
    /// </summary>
    bool HasThumbnail { get; set; }

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
