using Raven.Client.Documents;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class MediaApi : IMediaApi
  {
    protected IDocumentStore Raven { get; set; }

    protected IPaths Paths { get; set; }

    private const char PATH_SEPARATOR = '/';

    private const string PATH_PREFIX = "/uploads";

    private const char SEP = ',';

    private const string THUMB_EXTENSION = ".thumb";

    private const string UPLOAD_PREFIX = "upload:";

    private string[] ImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".webp", ".jfif" };


    public MediaApi(IDocumentStore raven, IPaths paths)
    {
      Raven = raven;
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

      // generate new id
      media.Id = Raven.Id();

      // get bytes for media content
      string byteString = media.Source.Split(SEP)[1];
      byte[] bytes = Convert.FromBase64String(byteString);

      // build folder and full file path
      string folderPath = Path.Combine(Paths.Media, media.Id);
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
            image.Save(filePath.Replace(extension, THUMB_EXTENSION + extension));

            media.HasThumbnail = true;
          }
        }

        // set new properties
        media.LastModifiedDate = DateTimeOffset.Now;
        media.Source = Path.Combine(PATH_PREFIX, media.Id, media.Name).Replace(Path.DirectorySeparatorChar, PATH_SEPARATOR);
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


    /// <inheritdoc />
    public async Task Cleanup()
    {
      await Task.Delay(0);
    }
  }


  public interface IMediaApi
  {
    /// <summary>
    /// Generates a persistent file from an upload,
    /// in case the file is not persisted yet
    /// </summary>
    bool Upload(IMedia media, out bool uploaded, out string errorMessage);

    /// <summary>
    /// Clean-up all media based on stored database information
    /// </summary>
    Task Cleanup();
  }
}
