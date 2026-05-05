using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Web.Caching;

namespace Mixtape.Media;

public class MediaOptions
{
  public string FolderPath { get; set; } = string.Empty;

  public string PublicPathPrefix { get; set; } = string.Empty;

  public List<string> AllowedOtherFileExtensions { get; set; }

  public List<string> AllowedImageFileExtensions { get; set; }

  public Dictionary<string, ThumbnailOptions> Thumbnails { get; set; }

  public ImageSharpOptions ImageSharp { get; set; } = new();
}


public class ImageSharpOptions
{
  public ImagingSharpRemoteCacheOptions RemoteCache { get; set; } = new();

  public PhysicalFileSystemCacheOptions Cache { get; set; } = new();

  public Dictionary<string, string[]> Presets { get; set; } = [];

  public string[] DefaultCommands { get; set; } = [];
}


public class ImagingSharpRemoteCacheOptions
{
  public bool Enabled { get; set; } = true;

  public int ExpiresInHours { get; set; } = 168;

  public string MediaFolder { get; set; } = "/media/_remote/";

  public string KeyMapFile { get; set; } = "/Config/remotekeys.json";
}


public class ThumbnailOptions
{
  public string Extension { get; set; }

  public IImageEncoder Encoder { get; set; }

  public Action<IImageProcessingContext> Mutate { get; set; }
}