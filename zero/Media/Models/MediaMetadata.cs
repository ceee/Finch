namespace zero.Media;

/// <summary>
/// Metadata for images/videos
/// </summary>
public class MediaMetadata
{
  /// <summary>
  /// Optional focal point for an image
  /// </summary>
  public MediaFocalPoint FocalPoint { get; set; }

  /// <summary>
  /// Width in pixels
  /// </summary>
  public int Width { get; set; }

  /// <summary>
  /// Height in pixels
  /// </summary>
  public int Height { get; set; }

  /// <summary>
  /// Whether this image contains transparent pixels
  /// </summary>
  public bool HasTransparency { get; set; }
}