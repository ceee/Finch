namespace Mixtape.Media;

/// <summary>
/// The focal point sets the point of interest in an image with x/y coordinates from 0-1
/// </summary>
public class MediaFocalPoint
{
  public decimal Left { get; set; }

  public decimal Top { get; set; }

  public bool NotDefault() => Left != 0.5m || Top != 0.5m;
}
