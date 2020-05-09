using zero.Core.Entities;
using zero.Core.Mapper;
using zero.Web.Models;

namespace zero.Web.Mapper
{
  public class TranslationMapperConfig : IMapperConfig
  {
    /// <inheritdoc />
    public void Configure(IMapper config)
    {
      config.CreateMap<Translation, TranslationEditModel>((source, target) =>
      {
        target.Id = source.Id;
        target.CreatedDate = source.CreatedDate;
        target.Key = source.Key;
        target.Value = source.Value;
        target.Display = source.Display;
      });

      config.CreateMap<TranslationEditModel, Translation>((source, target) =>
      {
        target.IsActive = true;
        target.Key = source.Key;
        target.Value = source.Value;
        target.Display = source.Display;
      });

      config.CreateMap<Translation, TranslationListModel>((source, target) =>
      {
        target.Id = source.Id;
        target.CreatedDate = source.CreatedDate;
        target.Key = source.Key;
        target.Value = source.Value;
      });
    }
  }
}
