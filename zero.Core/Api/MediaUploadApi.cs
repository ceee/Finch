using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class MediaUploadApi : IMediaUploadApi
  {
    protected IPaths Paths { get; set; }

    private const char PATH_SEPARATOR = '/';

    private const string PATH_PREFIX = "/uploads";

    private const char SEP = ',';

    private const string THUMB_EXTENSION = ".thumb";

    private const string PREVIEW_EXTENSION = ".preview";

    private const string UPLOAD_PREFIX = "upload:";

    private string[] ImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".webp", ".jfif" };


    public MediaUploadApi(IPaths paths)
    {
      Paths = paths;
    }


    /// <inheritdoc />
    public async Task<Media> Upload(IFormFile file, string folderId, CancellationToken cancellationToken = default)
    {
      Media media = new Media();

      // generate file id which is used as the folder name on disk
      media.FileId = Guid.NewGuid().ToString();
      media.FolderId = folderId;

      // generate file name
      media.Name = Safenames.File(file.FileName);

      // build folder and full file path
      string folderPath = Path.Combine(Paths.Media, media.FileId);
      string filePath = Path.Combine(folderPath, media.Name);
      string extension = Path.GetExtension(filePath);

      // find media type
      media.Type = ImageExtensions.Contains(extension, StringComparer.InvariantCultureIgnoreCase) ? MediaType.Image : MediaType.File;

      Paths.Create(folderPath);

      // write media file to disk
      using (var stream = File.Create(filePath))
      {
        await file.CopyToAsync(stream, cancellationToken);
      }

      // write additional image data + thumbnail
      if (media.Type == MediaType.Image)
      {
        using (Image<Rgba32> image = Image.Load<Rgba32>(filePath))
        {
          media.Dimension = new MediaDimension()
          {
            Width = image.Width,
            Height = image.Height
          };

          image.Mutate(x => x.Resize(new ResizeOptions()
          {
            Size = new Size(210, 210),
            Mode = ResizeMode.Min
          }));

          string thumbFileName = media.Name.TrimEnd(extension) + PREVIEW_EXTENSION + extension;
          image.Save(Path.Combine(Paths.Media, media.FileId, thumbFileName));
          media.PreviewSource = Path.Combine(PATH_PREFIX, media.FileId, thumbFileName).Replace(Path.DirectorySeparatorChar, PATH_SEPARATOR);

          image.Mutate(x => x.Resize(new ResizeOptions()
          {
            Size = new Size(100, 100),
            Mode = ResizeMode.Max
          }));

          thumbFileName = media.Name.TrimEnd(extension) + THUMB_EXTENSION + extension;
          image.Save(Path.Combine(Paths.Media, media.FileId, thumbFileName));
          media.ThumbnailSource = Path.Combine(PATH_PREFIX, media.FileId, thumbFileName).Replace(Path.DirectorySeparatorChar, PATH_SEPARATOR);
        }
      }

      // set new properties
      media.LastModifiedDate = DateTimeOffset.Now;
      media.Source = Path.Combine(PATH_PREFIX, media.FileId, media.Name).Replace(Path.DirectorySeparatorChar, PATH_SEPARATOR);
      media.Size = file.Length;

      return media;
    }
  }


  public interface IMediaUploadApi
  {
    /// <summary>
    /// Uploads a file to the media folder
    /// </summary>
    Task<Media> Upload(IFormFile file, string folderId, CancellationToken cancellationToken = default);
  }
}
