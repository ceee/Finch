using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Web.Caching;

namespace zero.Media;

public class MediaOptions
{
  public string FolderPath { get; set; }

  public string PublicPathPrefix { get; set; } = string.Empty;

  public List<string> AllowedOtherFileExtensions { get; set; }

  public List<string> AllowedImageFileExtensions { get; set; }

  public Dictionary<string, ResizeOptions> Thumbnails { get; set; }

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

  public int ExpiresInHours { get; set; } = -1;

  public string MediaFolder { get; set; } = "/media/_remote/";

  public string KeyMapFile { get; set; } = "/Config/remotekeys.json";
}