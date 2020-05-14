using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using zero.Core.Entities;
using zero.Core.Utils;

namespace zero.Core.Api
{
  public class MediaUpload : IMediaUpload
  {
    protected IPaths Paths { get; set; }

    private const char PATH_SEPARATOR = '/';

    private const string PATH_PREFIX = "/uploads";

    private const char SEP = ',';

    private const string THUMB_EXTENSION = ".thumb";

    private const string UPLOAD_PREFIX = "upload:";

    private string[] ImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".webp", ".jfif" };


    public MediaUpload(IPaths paths)
    {
      Paths = paths;
    }


    /// <inheritdoc />
    public bool Upload(IMedia media, out bool uploaded, out string errorMessage)
    {
      errorMessage = null;
      uploaded = false;

      // do not upload already persisted files
      if (media == null || (!String.IsNullOrWhiteSpace(media.Id) && !media.Id.StartsWith(UPLOAD_PREFIX)))
      {
        return true;
      }

      // set id to null in case it was prefixed to signal an upload
      media.Id = null;

      // generate file id which is used as the folder name on disk
      media.FileId = IdGenerator.Create();

      // get bytes for media content
      string byteString = media.Source.Split(SEP)[1];
      byte[] bytes = Convert.FromBase64String(byteString);

      // build folder and full file path
      string folderPath = Path.Combine(Paths.Media, media.FileId);
      string filePath = Path.Combine(folderPath, media.Name);

      Paths.Create(folderPath);

      try
      {
        // write media file to disk
        File.WriteAllBytes(filePath, bytes);

        // get file extension
        string extension = Path.GetExtension(filePath);

        // write additional image data + thumbnail
        if (ImageExtensions.Contains(extension, StringComparer.InvariantCultureIgnoreCase))
        {
          using (Image<Rgba32> image = Image.Load(bytes))
          {
            media.Dimension = new MediaDimension()
            {
              Width = image.Width,
              Height = image.Height
            };

            image.Mutate(x => x.Resize(new ResizeOptions()
            {
              Size = new Size(100, 100),
              Mode = ResizeMode.Max
            }));

            string thumbFileName = media.Name.Replace(extension, THUMB_EXTENSION + extension);
            string thumbSourcePath = Path.Combine(Paths.Media, media.FileId, thumbFileName);

            image.Save(thumbSourcePath);

            media.ThumbnailSource = Path.Combine(PATH_PREFIX, media.FileId, thumbFileName).Replace(Path.DirectorySeparatorChar, PATH_SEPARATOR);
          }
        }

        // set new properties
        media.LastModifiedDate = DateTimeOffset.Now;
        media.Source = Path.Combine(PATH_PREFIX, media.FileId, media.Name).Replace(Path.DirectorySeparatorChar, PATH_SEPARATOR);
        media.Size = bytes.Length;

        uploaded = true;
      }
      catch (Exception ex)
      {
        errorMessage = ex.Message; // TODO correct message
        return false;
      }

      return true;
    }
  }


  public interface IMediaUpload
  {
    /// <summary>
    /// Generates a persistent file from an upload,
    /// in case the file is not persisted yet
    /// </summary>
    bool Upload(IMedia media, out bool uploaded, out string errorMessage);
  }
}
