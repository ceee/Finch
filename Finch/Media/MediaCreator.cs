using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Finch.Media;

public class MediaCreator(IMediaFileSystem fileSystem, IFinchOptions options, ILogger<IMediaCreator> logger) : IMediaCreator
{
  protected IMediaFileSystem FileSystem { get; set; } = fileSystem;

  protected MediaOptions Options { get; set; } = options.For<MediaOptions>();


  /// <inheritdoc />
  public virtual async Task<Result<Media>> UploadFile(Stream fileStream, string filename, string folderId = null, CancellationToken cancellationToken = default)
  {
    string fileExtension = Path.GetExtension(filename);
    string normalizedFilename = Safenames.File(filename);

    bool isImage = Options.AllowedImageFileExtensions.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase);
    bool isDocument = !isImage && Options.AllowedOtherFileExtensions.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase);

    if (!isImage && !isDocument)
    {
      // TODO error
      return Result<Media>.Fail("This file type is not supported");
    }

    Media model = new();
    model.Name = normalizedFilename;
    model.ParentId = folderId;
    model.IsFolder = false;

    // create directory which hosts the media file
    // the media directory is a flat folder where each folder contains one media file (+ thumbs)
    string directory = await CreateDirectory(cancellationToken);
    model.FileId = directory;

    // the path is a combination of the folder name + filename, e.g. 129021309123/myfile.jpg
    model.Path = directory + '/' + normalizedFilename;

    // store the file in the attached file system
    // we do not need to check if the file already exists in the file system
    // as we only use newly created directories, therefore a collision shouldn't happen
    await FileSystem.CreateFile(model.Path, fileStream, cancellationToken: cancellationToken);

    // we need file metadata to get info about file size and the physical path for image modification
    IFileMeta fileInfo = await FileSystem.GetFileInfo(model.Path, cancellationToken);
    model.Size = fileInfo.Length;
    
    logger.LogInformation("Created media item {path} ({size})", model.Path, model.Size.GetFileSize());

    if (isImage)
    {
      // load image to create thumbnails
      using Image<Rgba32> image = await Image.LoadAsync<Rgba32>(fileInfo.AbsolutePath, cancellationToken);
      model.Metadata = GetImageMetadata(image.Metadata, image.Size);

      string extension = Path.GetExtension(model.Path);

      if (Options.Thumbnails != null)
      {
        foreach ((string key, ThumbnailOptions opts) in Options.Thumbnails)
        {
          Image<Rgba32> imageFrame = image.Frames.Count > 1 ? image.Frames.CloneFrame(0) : image.Clone();
          IImageEncoder encoder = opts.Encoder ?? new WebpEncoder();

          if (opts.Mutate != null)
          {
            imageFrame.Mutate(opts.Mutate);
          }

          using MemoryStream stream = new();
          await imageFrame.SaveAsync(stream, encoder, cancellationToken);

          stream.Position = 0;

          string thumbFilename = normalizedFilename.TrimEnd(extension) + "." + Safenames.File(key) + opts.Extension.Or(".webp");
          string path = directory + '/' + thumbFilename;

          await FileSystem.CreateFile(path, stream, cancellationToken: cancellationToken);

          model.Thumbnails[key] = path;
        }
      }
    }

    return Result<Media>.Success(model);
  }


  /// <inheritdoc />
  protected virtual MediaMetadata GetImageMetadata(ImageMetadata metadata, Size size)
  {
    PngMetadata pngMetadata = metadata.GetPngMetadata();
    //WebpMetadata webpMetadata = image.Metadata.GetWebpMetadata();

    return new MediaMetadata()
    {
      Width = size.Width,
      Height = size.Height,
      //ImageTakenDate = new DateTimeOffset(image.Metadata.IccProfile?.Header?.CreationDate ?? DateTime.Now),
      //Dpi = image.Metadata.HorizontalResolution,
      //ColorSpace = image.Metadata.IccProfile?.Header?.DataColorSpace.ToString(),
      HasTransparency = pngMetadata?.TransparentColor.HasValue ?? false
      //Frames = image.Frames.Count
    };
  }


  /// <summary>
  /// Create a new directory for a file. 
  /// This method is collision-aware and repeats until a directory can be created.
  /// </summary>
  protected virtual async Task<string> CreateDirectory(CancellationToken cancellationToken = default)
  {
    try
    {
      string directoryName = GetNewDirectoryName();
      await FileSystem.CreateDirectory(directoryName, cancellationToken);
      logger.LogDebug("Created media directory {name}", directoryName);
      return directoryName;
    }
    catch (FileSystemException ex) when (ex.Message.Contains("already exists"))
    {
      return await CreateDirectory(cancellationToken);
    }
  }


  /// <summary>
  /// Builds a directory name for a new media item
  /// </summary>
  protected virtual string GetNewDirectoryName()
  {
    return Guid.NewGuid().ToString();
  }
}



public interface IMediaCreator
{
  /// <summary>
  /// Uploads a file by using the attached file system
  /// </summary>
  /// <returns>A temporary media file which can be persisted in a store</returns>
  Task<Result<Media>> UploadFile(Stream fileStream, string filename, string folderId = null, CancellationToken cancellationToken = default);
}