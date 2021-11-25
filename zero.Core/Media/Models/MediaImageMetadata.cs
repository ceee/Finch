namespace zero.Media;

/// <summary>
/// Metadata for images
/// </summary>
public class MediaImageMetadata
{
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