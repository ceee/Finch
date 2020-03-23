using System;

namespace unjo.Core.Entities
{
  /// <summary>
  /// A media file (can contain an image or other media like videos and documents)
  /// </summary>
  public class Media : DatabaseEntity
  {
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
    /// Filesize in bytes
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// Time the file has last changed
    /// </summary>
    public DateTimeOffset LastModifiedDate { get; set; }

    /// <summary>
    /// Optional focal point for an image
    /// </summary>
    public MediaFocalPoint FocalPoint { get; set; }
  }
}
