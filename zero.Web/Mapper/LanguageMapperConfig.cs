using zero.Core.Entities;
using zero.Web.Models;

namespace zero.Web.Mapper
{
  public class LanguageMapperConfig : IMapperConfig
  {
    /// <inheritdoc />
    public void Configure(IMapper config)
    {
      config.CreateMap<Language, LanguageEditModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.IsActive = source.IsActive;
        target.IsDefault = source.IsDefault;
        target.InheritedLanguageId = source.InheritedLanguageId;
        target.Code = source.Code;
        target.CreatedDate = source.CreatedDate;
      });

      config.CreateMap<LanguageEditModel, Language>((source, target) =>
      {
        target.Name = source.Name;
        target.IsActive = source.IsActive;
        target.IsDefault = source.IsDefault;
        target.InheritedLanguageId = source.InheritedLanguageId;
        target.Code = source.Code;
      });

      config.CreateMap<Language, LanguageListModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.IsActive = source.IsActive;
        target.IsDefault = source.IsDefault;
        target.InheritedLanguageId = source.InheritedLanguageId;
        target.Code = source.Code;
      });
    }
  }
}
