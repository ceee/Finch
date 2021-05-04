using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using zero.Core.Database.Indexes;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace zero.Core.Api
{
  public class MediaApi : BackofficeApi, IMediaApi
  {
    protected const char PATH_SEPARATOR = '/';

    protected const string PATH_PREFIX = "/uploads";

    protected const string THUMB_EXTENSION = ".thumb";

    protected const string PREVIEW_EXTENSION = ".preview";

    protected string[] ImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".webp", ".jfif", ".gif" };

    protected IPaths Paths { get; set; }


    public MediaApi(IBackofficeStore store, IPaths paths) : base(store)
    {
      Paths = paths;
    }


    /// <inheritdoc />
    public async Task<Media> GetById(string id)
    {
      return await GetById<Media>(id);
    }


    /// <inheritdoc />
    public async Task<string> GetSourceById(string id, bool thumb = false, bool isCoreDatabase = false)
    {
      using IAsyncDocumentSession session = isCoreDatabase ? Store.OpenCoreSession() : Store.OpenAsyncSession();

      Media media = await session.LoadAsync<Media>(id);

      if (media == null)
      {
        return null;
      }

      if (media.Source.StartsWith("url://"))
      {
        return media.Source.Substring(6) + "?preset=productMini";
      }

      return thumb ? (media.ThumbnailSource ?? media.Source) : media.Source;
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, Media>> GetById(IEnumerable<string> ids)
    {
      using (IAsyncDocumentSession session = Store.OpenAsyncSession())
      {
        return await session.LoadAsync<Media>(ids);
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<Media>> GetByQuery(MediaListQuery query)
    {
      query.SearchFor(entity => entity.Name);

      using (IAsyncDocumentSession session = Store.OpenAsyncSession())
      {
        return await session.Query<Media>()
          .WhereIf(x => x.FolderId == query.FolderId, !query.FolderId.IsNullOrEmpty(), x => x.FolderId == null)
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<MediaListItem>> GetListByQuery(MediaListItemQuery query)
    {
      bool hasSearch = !query.Search.IsNullOrWhiteSpace();
      bool isRoot = query.FolderId.IsNullOrWhiteSpace();

      query.SearchFor(entity => entity.Name);

      query.OrderQuery = q => q
        .OrderByDescending(x => x.IsFolder)
        .ThenBy(query.OrderBy, query.OrderIsDescending, query.OrderType == ListQueryOrderType.String ? OrderingType.String : OrderingType.Double);

      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      session.Advanced.MaxNumberOfRequestsPerSession = query.PageSize + 1;

      IRavenQueryable<MediaListItem> dbQuery = session.Query<MediaListItem, Media_ByParent>().ProjectInto<MediaListItem>();

      if (!hasSearch || !query.SearchIsGlobal)
      {
        dbQuery = dbQuery.WhereIf(x => x.ParentId == query.FolderId, !query.FolderId.IsNullOrEmpty(), x => x.ParentId == null);
      }

      ListResult<MediaListItem> result = await dbQuery.ToQueriedListAsyncX(query);

      foreach (MediaListItem item in result.Items)
      {
        if (item.IsFolder)
        {
          item.Children = await session.Query<MediaListItem, Media_ByParent>().CountAsync(x => x.ParentId == item.Id);
        }
      }

      // TODO this is only inserted when we have a core/shared database and a multi-tenancy setup
      if (isRoot)
      {
        result.Items.Insert(0, new MediaListItem()
        {
          Id = "shared",
          IsFolder = true
        });
      }

      return result;
    }


    /// <inheritdoc />
    public async Task<EntityResult<Media>> Save(Media model)
    {
      model.IsActive = true;
      return await SaveModel(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Media>> Move(string id, string parentId)
    {
      Media model = await GetById<Media>(id);
      MediaFolder parent = await GetById<MediaFolder>(parentId);

      if (model == null || (!parentId.IsNullOrEmpty() && parent == null))
      {
        return EntityResult<Media>.Fail("@errors.idnotfound");
      }

      model.FolderId = parent?.Id;

      return await Save(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Media>> Delete(string id)
    {
      return await DeleteById<Media>(id);
    }


    /// <inheritdoc />
    public async Task Cleanup()
    {
      await Task.Delay(0);
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

      // set new properties
      media.Source = Path.Combine(PATH_PREFIX, media.FileId, media.Name).Replace(Path.DirectorySeparatorChar, PATH_SEPARATOR);
      media.Size = file.Length;

      // write additional image data + thumbnail
      if (media.Type == MediaType.Image)
      {
        using (Image<Rgba32> image = Image.Load<Rgba32>(filePath))
        {
          media.ImageMeta = GetImageMeta(image);

          Image<Rgba32> imageFrame = media.ImageMeta.Frames > 1 ? image.Frames.CloneFrame(0) : image;

          media.PreviewSource = SaveThumbnail(media, imageFrame, PREVIEW_EXTENSION, new ResizeOptions()
          {
            Size = new Size(210, 210),
            Mode = ResizeMode.Min
          });

          media.ThumbnailSource = SaveThumbnail(media, imageFrame, THUMB_EXTENSION, new ResizeOptions()
          {
            Size = new Size(100, 100),
            Mode = ResizeMode.Max
          });
        }
      }

      return media;
    }


    /// <summary>
    /// Saves a thumbnail of an image
    /// </summary>
    string SaveThumbnail(Media media, Image<Rgba32> image, string extensionPrefix, ResizeOptions resizeOptions)
    {
      string extension = Path.GetExtension(media.Source);

      image.Mutate(x => x.Resize(resizeOptions));

      string thumbFileName = media.Name.TrimEnd(extension) + extensionPrefix + extension;
      image.Save(Path.Combine(Paths.Media, media.FileId, thumbFileName));
      return Path.Combine(PATH_PREFIX, media.FileId, thumbFileName).Replace(Path.DirectorySeparatorChar, PATH_SEPARATOR);
    }


    /// <summary>
    /// Create image data if available
    /// </summary>
    MediaImageMeta GetImageMeta(Image<Rgba32> image)
    {
      var pngMetadata = image.Metadata.GetPngMetadata();

      return new MediaImageMeta()
      {
        Width = image.Width,
        Height = image.Height,
        CreatedDate = new DateTimeOffset(image.Metadata.IccProfile?.Header?.CreationDate ?? DateTime.Now),
        DPI = image.Metadata.HorizontalResolution,
        ColorSpace = image.Metadata.IccProfile?.Header?.DataColorSpace.ToString(),
        HasTransparency = pngMetadata?.HasTransparency ?? false,
        Frames = image.Frames.Count
      };
    }
  }


  public interface IMediaApi : IBackofficeApi
  {
    /// <summary>
    /// Get media by Id
    /// </summary>
    Task<Media> GetById(string id);

    /// <summary>
    /// Get media source by Id
    /// </summary>
    Task<string> GetSourceById(string id, bool thumb = false, bool isCoreDatabase = false);

    /// <summary>
    /// Get media by ids
    /// </summary>
    Task<Dictionary<string, Media>> GetById(IEnumerable<string> ids);

    /// <summary>
    /// Get all available media items with query
    /// </summary>
    Task<ListResult<Media>> GetByQuery(MediaListQuery query);

    /// <summary>
    /// Get all available media items (including folders) with query
    /// </summary>
    Task<ListResult<MediaListItem>> GetListByQuery(MediaListItemQuery query);

    /// <summary>
    /// Creates or updates a media item
    /// </summary>
    Task<EntityResult<Media>> Save(Media model);

    /// <summary>
    /// Move a file to a new parent
    /// </summary>
    Task<EntityResult<Media>> Move(string id, string parentId);

    /// <summary>
    /// Deletes a media item by Id
    /// </summary>
    Task<EntityResult<Media>> Delete(string id);

    /// <summary>
    /// Clean-up all media based on stored database information
    /// </summary>
    Task Cleanup();

    /// <summary>
    /// Uploads a file to the media folder
    /// </summary>
    Task<Media> Upload(IFormFile file, string folderId, CancellationToken cancellationToken = default);
  }
}
