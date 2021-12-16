namespace zero.Api.Endpoints.Media;

public class MediaMapperProfile : ZeroMapperProfile
{
  protected IMediaFileSystem FileSystem { get; private set; }


  public MediaMapperProfile(IMediaFileSystem fileSystem)
  {
    FileSystem = fileSystem;
  }


  public override void Configure(IZeroMapper mapper)
  {
    mapper.Define<zero.Media.Media, MediaBasic>((source, ctx) => new(), Map);
    mapper.Define<zero.Media.Media, MediaEdit>((source, ctx) => new(), Map);
    mapper.Define<zero.Media.Media, MediaFileEdit>((source, ctx) => new(), Map);
    mapper.Define<zero.Media.Media, MediaFolderEdit>((source, ctx) => new(), Map);
    mapper.Define<MediaSave, zero.Media.Media>((source, ctx) => new(), Map);
  }


  protected virtual void Map(zero.Media.Media source, MediaBasic target, IZeroMapperContext ctx)
  {
    target.Id = source.Id;
    target.Name = source.Name;
    target.ParentId = source.ParentId;
    target.IsFolder = source.Type == MediaType.Folder;
    target.Children = 0;
    target.Size = source.Size;

    if (source.Thumbnails.Any())
    {
      string path = source.Thumbnails.GetValueOrDefault("preview");
      target.Image = path.IsNullOrEmpty() ? null : FileSystem.MapToPublicPath(path);
    }
  }

  protected virtual void Map(zero.Media.Media source, MediaEdit target, IZeroMapperContext ctx)
  {
    this.MapDisplayData(source, target);
    target.Type = source.Type;
    target.ParentId = source.ParentId;
  }


  protected virtual void Map(zero.Media.Media source, MediaFolderEdit target, IZeroMapperContext ctx)
  {
    Map(source, (MediaEdit)target, ctx);
  }


  protected virtual void Map(zero.Media.Media source, MediaFileEdit target, IZeroMapperContext ctx)
  {
    Map(source, (MediaEdit)target, ctx);
    target.AlternativeText = source.AlternativeText;
    target.Caption = source.Caption;
    target.Path = source.Path;
    target.Thumbnails = source.Thumbnails;
    target.Size = source.Size;
    target.ImageMeta = source.ImageMeta;
    target.FocalPoint = source.FocalPoint;
}


  protected virtual void Map(MediaSave source, zero.Media.Media target, IZeroMapperContext ctx)
  {
    this.MapSaveData(source, target);
    
  }
}