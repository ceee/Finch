using SixLabors.ImageSharp.Processing;

namespace zero.Media;

public class MediaOptions
{
  public string FolderPath { get; set; }

  public string PublicPathPrefix { get; set; }

  public List<string> AllowedOtherFileExtensions { get; set; }

  public List<string> AllowedImageFileExtensions { get; set; }

  public Dictionary<string, ResizeOptions> Thumbnails { get; set; }
}