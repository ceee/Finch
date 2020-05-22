using zero.Core.Entities;
using zero.Core.Mapper;
using zero.Web.Models;

namespace zero.Web.Mapper
{
  public class ApplicationMapperConfig : IMapperConfig
  {
    /// <inheritdoc />
    public void Configure(IMapper config)
    {
      config.CreateMap<Application, ApplicationEditModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.FullName = source.FullName;
        target.Email = source.Email;
        target.IsActive = source.IsActive;
        target.CreatedDate = source.CreatedDate;
        //target.Domains = source.Domains;
        target.ImageId = source.ImageId;
        target.IconId = source.IconId;
        target.Features = source.Features;
      });

      config.CreateMap<ApplicationEditModel, Application>((source, target) =>
      {
        target.Name = source.Name;
        target.FullName = source.FullName;
        target.Email = source.Email;
        target.IsActive = source.IsActive;
        //target.Domains = source.Domains;
        target.ImageId = source.ImageId;
        target.IconId = source.IconId;
        target.Features = source.Features;
      });

      config.CreateMap<Application, ApplicationListModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.FullName = source.FullName;
        target.IsActive = source.IsActive;
        //target.Domains = source.Domains;
        target.ImageId = source.ImageId;
      });
    }
  }
}
