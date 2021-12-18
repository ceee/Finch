namespace zero.Media;

/// <summary>
/// Metadata for images
/// </summary>
public class MediaImageMetadata
{
  /// <summary>
  /// Alternative text which is used when the image can't be loaded
  /// </summary>
  public string AlternativeText { get; set; }

  /// <summary>
  /// Define custom thumbnails which are generated on upload 
  /// (see IZeroOptions.For<MediaOptions>().Thumbnails)
  /// </summary>
  public Dictionary<string, string> Thumbnails { get; set; } = new();

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
  /// Resolution factor
  /// </summary>
  public double DPI { get; set; }

  /// <summary>
  /// Date the image was taken
  /// </summary>
  public DateTimeOffset? ImageTakenDate { get; set; }

  /// <summary>
  /// Original color space of the image
  /// </summary>
  public string ColorSpace { get; set; }

  /// <summary>
  /// Whether this image contains transparent pixels
  /// </summary>
  public bool HasTransparency { get; set; }

  /// <summary>
  /// How many frames contains this image (for animation)
  /// </summary>
  public int Frames { get; set; } = 1;
}