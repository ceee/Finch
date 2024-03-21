using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;
using System.IO;

namespace zero.Media;

public class StaticMediaCreator : MediaCreator, IStaticMediaCreator
{
  protected IMemoryCache Cache { get; set; }

  protected IFileProvider FileProvider { get; set; }


  public StaticMediaCreator(IMediaFileSystem fileSystem, IZeroOptions options, IWebHostEnvironment hostingEnvironment, MediaMetadataCache cacheProvider) : base(fileSystem, options)
  {
    FileProvider = hostingEnvironment.WebRootFileProvider;
    Cache = cacheProvider.Cache;
  }



  /// <inheritdoc />
  public virtual async Task<Result<Media>> GetMedia(string path, CancellationToken cancellationToken = default)
  {
    if (Cache.TryGetValue(path, out Media media))
    {
      return Result<Media>.Success(media);
    }

    string filename = Path.GetFileName(path);
    string fileExtension = Path.GetExtension(filename);

    bool isImage = Options.AllowedImageFileExtensions.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase);
    bool isDocument = !isImage && Options.AllowedOtherFileExtensions.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase);

    if (!isImage && !isDocument)
    {
      return Result<Media>.Fail("This file type is not supported");
    }

    Media model = new();
    model.Name = filename;
    model.Path = path;
    model.IsFolder = false;

    // we need file metadata to get info about file size and the physical path for image modification
    IFileMeta fileInfo = await FileSystem.GetFileInfo(model.Path, cancellationToken);
    model.Size = fileInfo.Length;

    if (isImage)
    {
      IImageInfo info = await Image.IdentifyAsync(fileInfo.AbsolutePath, cancellationToken);
      model.Metadata = GetImageMetadata(info);
    }

    MemoryCacheEntryOptions cacheEntryOptions = new();
    cacheEntryOptions.AddExpirationToken(FileProvider.Watch(fileInfo.AbsolutePath));
    cacheEntryOptions.SetSize(1);
    Cache.Set(path, model, cacheEntryOptions);

    return Result<Media>.Success(model);
  }
}



public interface IStaticMediaCreator
{
  /// <summary>
  /// Builds a media file from a path
  /// </summary>
  Task<Result<Media>> GetMedia(string path, CancellationToken cancellationToken = default);
}