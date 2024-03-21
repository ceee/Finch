using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;

namespace zero.Media;


public class ImageDimensionReader : IImageDimensionReader
{
  protected IMediaFileSystem FileSystem { get; set; }

  protected IFileProvider FileProvider { get; set; }

  protected IMemoryCache Cache { get; set; }


  public ImageDimensionReader(IMediaFileSystem fileSystem, IWebHostEnvironment hostingEnvironment, MediaMetadataCache cacheProvider)
  {
    FileSystem = fileSystem;
    FileProvider = hostingEnvironment.WebRootFileProvider;
    Cache = cacheProvider.Cache;
  }


  /// <inheritdoc />
  public System.Drawing.Size GetSize(string path)
  {
    if (Cache.TryGetValue(path, out System.Drawing.Size value))
    {
      return value;
    }

    string resolvedPath = FileSystem.Map(path);

    try
    {
      IImageInfo info = Image.Identify(resolvedPath);
      value = new(info.Width, info.Height);
    }
    catch (Exception)
    {
      return default;
    }

    MemoryCacheEntryOptions cacheEntryOptions = new();
    cacheEntryOptions.AddExpirationToken(FileProvider.Watch(resolvedPath));
    cacheEntryOptions.SetSize(1);
    Cache.Set(path, value, cacheEntryOptions);

    return value;
  }


  /// <inheritdoc />
  public async Task<System.Drawing.Size> GetSizeAsync(string path)
  {
    if (path.IsNullOrEmpty())
    {
      return default;
    }

    if (Cache.TryGetValue(path, out System.Drawing.Size value))
    {
      return value;
    }

    string resolvedPath = FileSystem.Map(path);

    try
    {
      IImageInfo info = await Image.IdentifyAsync(resolvedPath);
      value = new(info.Width, info.Height);
    }
    catch (Exception)
    {
      return default;
    }

    MemoryCacheEntryOptions cacheEntryOptions = new();
    cacheEntryOptions.AddExpirationToken(FileProvider.Watch(resolvedPath));
    cacheEntryOptions.SetSize(1);
    Cache.Set(path, value, cacheEntryOptions);

    return value;
  }
}



public interface IImageDimensionReader
{
  /// <summary>
  /// Get dimension for an image, when found in metadata.
  /// </summary>
  /// <param name="path">The path of the file to which version should be added.</param>
  /// <returns>Dimensions with width and height.</returns>
  System.Drawing.Size GetSize(string path);

  /// <summary>
  /// Get dimension for an image, when found in metadata.
  /// </summary>
  /// <param name="path">The path of the file to which version should be added.</param>
  /// <returns>Dimensions with width and height.</returns>
  Task<System.Drawing.Size> GetSizeAsync(string path);
}