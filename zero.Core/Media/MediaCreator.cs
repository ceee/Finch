using Shorthand.ImageSharp.WebP;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace zero.Media;

public class MediaCreator : IMediaCreator
{
  protected IMediaFileSystem FileSystem { get; set; }

  protected IMediaStore Store { get; set; }

  protected MediaOptions Options { get; set; }


  public MediaCreator(IMediaFileSystem fileSystem, IMediaStore store, IZeroOptions options)
  {
    FileSystem = fileSystem;
    Store = store;
    Options = options.For<MediaOptions>();
  }


  /// <inheritdoc />
  public async Task<Result<Media>> UploadFile(Stream fileStream, string filename, string folderId = null, CancellationToken cancellationToken = default)
  {
    string fileExtension = Path.GetExtension(filename);
    string normalizedFilename = Safenames.File(filename);

    bool isImage = Options.AllowedImageFileExtensions.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase);
    bool isDocument = !isImage && Options.AllowedOtherFileExtensions.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase);

    if (!isImage && !isDocument)
    {
      // TODO error
      return Result<Media>.Fail("ERROR");
    }

    Media model = await Store.Empty();
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

    if (isImage)
    {
      using Image<Rgba32> image = await Image.LoadAsync<Rgba32>(fileInfo.AbsolutePath);
      model.ImageMeta = GetImageMetadata(image);

      string extension = Path.GetExtension(model.Path);

      foreach ((string key, ResizeOptions opts) in Options.Thumbnails)
      {
        Image<Rgba32> imageFrame = image.Frames.Count > 1 ? image.Frames.CloneFrame(0) : image.Clone();
        imageFrame.Mutate(x => x.Resize(opts));

        using MemoryStream stream = new();
        //image.Save(stream, new WebPEncoder()
        //{
        //  Quality = 70
        //});
        await imageFrame.SaveAsync(stream, new JpegEncoder()
        {
          Quality = 80
        }, cancellationToken);

        stream.Position = 0;

        string thumbFilename = normalizedFilename.TrimEnd(extension) + "." + Safenames.File(key) + ".jpg";
        string path = directory + '/' + thumbFilename;

        await FileSystem.CreateFile(path, stream, cancellationToken: cancellationToken);

        model.ImageMeta.Thumbnails[key] = path;
      }
    }

    return Result<Media>.Success(model);
  }


  /// <inheritdoc />
  protected virtual MediaImageMetadata GetImageMetadata(Image<Rgba32> image)
  {
    PngMetadata pngMetadata = image.Metadata.GetPngMetadata();

    return new MediaImageMetadata()
    {
      Width = image.Width,
      Height = image.Height,
      ImageTakenDate = new DateTimeOffset(image.Metadata.IccProfile?.Header?.CreationDate ?? DateTime.Now),
      DPI = image.Metadata.HorizontalResolution,
      ColorSpace = image.Metadata.IccProfile?.Header?.DataColorSpace.ToString(),
      HasTransparency = pngMetadata?.HasTransparency ?? false,
      Frames = image.Frames.Count
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