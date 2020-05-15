using zero.Core.Entities;
using zero.Core.Mapper;
using zero.Web.Models;

namespace zero.Web.Mapper
{
  public class MediaMapperConfig : IMapperConfig
  {
    /// <inheritdoc />
    public void Configure(IMapper config)
    {
      config.CreateMap<Media, MediaEditModel>((source, target) =>
      {
        target.Id = source.Id;
        //target.Name = source.Name;
        //target.FullName = source.FullName;
        //target.Email = source.Email;
        target.IsActive = source.IsActive;
        target.CreatedDate = source.CreatedDate;
        //target.Domains = source.Domains;
        //target.ImageId = source.ImageId;
        //target.IconId = source.IconId;
        //target.Features = source.Features;
      });

      config.CreateMap<MediaEditModel, Media>((source, target) =>
      {
        //target.Name = source.Name;
        //target.FullName = source.FullName;
        //target.Email = source.Email;
        //target.IsActive = source.IsActive;
        //target.Domains = source.Domains;
        //target.ImageId = source.ImageId;
        //target.IconId = source.IconId;
        //target.Features = source.Features;
      });

      config.CreateMap<Media, MediaListModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.IsFolder = false;
        target.Size = source.Size;
        target.Source = source.Source;
        target.ThumbnailSource = source.ThumbnailSource;
      });

      config.CreateMap<MediaFolder, MediaListModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.IsFolder = true;
      });
    }
  }
}
