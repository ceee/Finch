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
using zero.Core.Database.Indexes;

namespace zero.Core.Collections
{
  public class MediaCollection : EntityStore<Media>, IMediaCollection
  {
    /// <inheritdoc />
    public IMediaFolderCollection Folders { get; protected set; }

    public MediaCollection(IStoreContext<Media> context, IMediaFolderCollection folders, IPaths paths) : base(context)
    {
      Options = new(true);
      Folders = folders;
      //PreSave = model => model.IsActive = true;
      Paths = paths;
    }


    protected const char PATH_SEPARATOR = '/';

    protected const string PATH_PREFIX = "/uploads";

    protected const string THUMB_EXTENSION = ".thumb";

    protected const string PREVIEW_EXTENSION = ".preview";

    protected string[] ImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".webp", ".jfif", ".gif" };

    protected IPaths Paths { get; set; }


    /// <inheritdoc />
    public override Task<EntityResult<Media>> Save(Media model)
    {
      model.IsActive = true;
      return base.Save(model);
    }


    /// <inheritdoc />
    public virtual async Task<string> GetSource(string id, MediaSourceSize size = MediaSourceSize.Original)
    {
      Media media = await Session.LoadAsync<Media>(id);

      if (media == null)
      {
        return null;
      }

      if (size == MediaSourceSize.Thumbnail && media.ThumbnailSource.HasValue())
      {
        return media.ThumbnailSource;
      }
      if (size == MediaSourceSize.Preview && media.PreviewSource.HasValue())
      {
        return media.PreviewSource;
      }

      return media.Source;
    }


    /// <inheritdoc />
    public virtual async Task<Paged<Media>> Load(MediaListQuery query)
    {
      query.SearchFor(entity => entity.Name);

      return await Session.Query<Media>()
        .WhereIf(x => x.FolderId == query.FolderId, !query.FolderId.IsNullOrEmpty(), x => x.FolderId == null)
        .ToQueriedListAsync(query);
    }


    /// <inheritdoc />
    public virtual async Task<EntityResult<Media>> Move(string id, string parentId)
    {
      Media model = await Load(id);
      MediaFolder parent = await Session.LoadAsync<MediaFolder>(parentId);

      if (model == null || (!parentId.IsNullOrEmpty() && parent == null))
      {
        return EntityResult<Media>.Fail("@errors.idnotfound");
      }

      model.FolderId = parent?.Id;

      return await Save(model);
    }


    /// <inheritdoc />
    public virtual async Task<Media> Upload(IFormFile file, string folderId, CancellationToken cancellationToken = default)
    {
      Media media = await Empty();

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
    public virtual async Task<Paged<MediaListItem>> Load(MediaListItemQuery query)
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

      Paged<MediaListItem> result = await dbQuery.ToQueriedListAsyncX(query);

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


    /// <inheritdoc />
    protected override void ValidationRules(ZeroValidator<Media> validator)
    {
      validator.RuleFor(x => x.Name).Length(2, 80);
      validator.RuleFor(x => x.IsActive).Equal(true);
    }


    /// <summary>
    /// Saves a thumbnail of an image
    /// </summary>
    protected virtual string SaveThumbnail(Media media, Image<Rgba32> image, string extensionPrefix, ResizeOptions resizeOptions)
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
    protected virtual MediaImageMeta GetImageMeta(Image<Rgba32> image)
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


  public interface IMediaCollection : IEntityStore<Media>
  {
    /// <summary>
    /// Media folder collection
    /// </summary>
    IMediaFolderCollection Folders { get; }

    /// <summary>
    /// Get media source by Id
    /// </summary>
    Task<string> GetSource(string id, MediaSourceSize size = MediaSourceSize.Original);

    /// <summary>
    /// Get all available media items with query
    /// </summary>
    Task<Paged<Media>> Load(MediaListQuery query);

    /// <summary>
    /// Get all available media items (including folders) with query
    /// </summary>
    Task<Paged<MediaListItem>> Load(MediaListItemQuery query);

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
