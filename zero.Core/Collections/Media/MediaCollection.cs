using FluentValidation;
using Microsoft.AspNetCore.Http;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Collections
{
  public class MediaCollection : CollectionBase<Media>, IMediaCollection
  {
    private IAsyncDocumentSession _coreSession;


    public MediaCollection(IZeroContext context, ICollectionInterceptorHandler interceptor, IValidator<Media> validator, IPaths paths) : base(context, interceptor, validator)
    {
      PreSave = model => model.IsActive = true;
      Paths = paths;
    }


    /// <summary>
    /// Create an an async document session
    /// </summary>
    protected IAsyncDocumentSession CoreSession
    {
      get
      {
        if (_coreSession != null)
        {
          return _coreSession;
        }
        _coreSession = Store.OpenCoreSession();
        _coreSession.Advanced.WaitForIndexesAfterSaveChanges(throwOnTimeout: false);
        return _coreSession;
      }
    }


    protected const char PATH_SEPARATOR = '/';

    protected const string PATH_PREFIX = "/uploads";

    protected const string THUMB_EXTENSION = ".thumb";

    protected const string PREVIEW_EXTENSION = ".preview";

    protected string[] ImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".webp", ".jfif", ".gif" };

    protected IPaths Paths { get; set; }


    public override Task<Media> GetById(string id)
    {
      ApplyScopeBasedOnId(id);
      return base.GetById(id);
    }


    public override async Task<Dictionary<string, Media>> GetByIds(params string[] ids)
    {
      string[] localIds = ids.Where(x => !x.StartsWith(Constants.Database.CoreIdPrefix)).ToArray();
      string[] coreIds = ids.Except(localIds).ToArray();

      Dictionary<string, Media> result = new Dictionary<string, Media>();
      Dictionary<string, Media> models = null;

      if (localIds.Length > 0)
      {
        models = await Session.LoadAsync<Media>(localIds);
        foreach (string id in localIds)
        {
          models.TryGetValue(id, out Media model);
          result.Add(id, model);
        }
      }
      if (coreIds.Length > 0)
      {
        models = await CoreSession.LoadAsync<Media>(coreIds);
        foreach (string id in coreIds)
        {
          models.TryGetValue(id, out Media model);
          result.Add(id, model);
        }
      }

      return result;
    }


    /// <inheritdoc />
    public async Task<string> GetSourceById(string id, bool thumb = false)
    {
      ApplyScopeBasedOnId(id);

      Media media = await Session.LoadAsync<Media>(id);

      if (media == null)
      {
        return null;
      }

      if (media.Source.StartsWith("url://"))
      {
        return media.Source.Substring(6) + "?preset=productMini"; // TODO remove this
      }

      return thumb ? (media.ThumbnailSource ?? media.Source) : media.Source;
    }


    /// <inheritdoc />
    public async Task<ListResult<Media>> GetByQuery(MediaListQuery query)
    {
      ApplyScopeBasedOnId(query.FolderId);

      query.SearchFor(entity => entity.Name);

      return await Query
        .WhereIf(x => x.FolderId == query.FolderId, !query.FolderId.IsNullOrEmpty(), x => x.FolderId == null)
        .ToQueriedListAsync(query);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Media>> Move(string id, string parentId)
    {
      Media model = await GetById(id);
      MediaFolder parent = await Session.LoadAsync<MediaFolder>(parentId);

      if (model == null || (!parentId.IsNullOrEmpty() && parent == null))
      {
        return EntityResult<Media>.Fail("@errors.idnotfound");
      }

      model.FolderId = parent?.Id;

      return await Save(model);
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


    /// <inheritdoc />
    public async Task<ListResult<MediaListItem>> GetListByQuery(MediaListItemQuery query)
    {
      bool hasSearch = !query.Search.IsNullOrWhiteSpace();
      bool isRoot = query.FolderId.IsNullOrWhiteSpace();

      query.SearchFor(entity => entity.Name);

      query.OrderQuery = q => q
        .OrderByDescending(x => x.IsFolder)
        .ThenBy(query.OrderBy, query.OrderIsDescending, query.OrderType == ListQueryOrderType.String ? OrderingType.String : OrderingType.Double);

      IRavenQueryable<MediaListItem> dbQuery = Session.Query<MediaListItem, Media_ByParent>().ProjectInto<MediaListItem>();

      if (!hasSearch || !query.SearchIsGlobal)
      {
        dbQuery = dbQuery.WhereIf(x => x.ParentId == query.FolderId, !query.FolderId.IsNullOrEmpty(), x => x.ParentId == null);
      }

      ListResult<MediaListItem> result = await dbQuery.ToQueriedListAsyncX(query);

      string[] ids = result.Items.Where(x => x.IsFolder).Select(x => x.Id).ToArray();

      List<Media_ByChildren.Result> children = await Session.Query<Media_ByChildren.Result, Media_ByChildren>()
        .Where(x => x.ParentId.In(ids))
        .ToListAsync();

      foreach (MediaListItem item in result.Items)
      {
        item.Children = children.FirstOrDefault(x => x.ParentId == item.Id)?.ChildrenCount ?? 0;
      }

      return result;
    }


    /// <summary>
    /// Applies the shared scope when an ID is prefixed with core.
    /// </summary>
    void ApplyScopeBasedOnId(string id)
    {
      if (!id.IsNullOrWhiteSpace() && id.StartsWith(Constants.Database.CoreIdPrefix, StringComparison.InvariantCultureIgnoreCase))
      {
        ApplyScope("shared");
      }
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


  public interface IMediaCollection : ICollectionBase<Media>
  {
    /// <summary>
    /// Get media source by Id
    /// </summary>
    Task<string> GetSourceById(string id, bool thumb = false);

    /// <summary>
    /// Get all available media items with query
    /// </summary>
    Task<ListResult<Media>> GetByQuery(MediaListQuery query);

    /// <summary>
    /// Get all available media items (including folders) with query
    /// </summary>
    Task<ListResult<MediaListItem>> GetListByQuery(MediaListItemQuery query);

    /// <summary>
    /// Move a file to a new parent
    /// </summary>
    Task<EntityResult<Media>> Move(string id, string parentId);

    /// <summary>
    /// Uploads a file to the media folder
    /// </summary>
    Task<Media> Upload(IFormFile file, string folderId, CancellationToken cancellationToken = default);
  }
}
