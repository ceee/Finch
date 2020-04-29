using Raven.Client.Documents;
using zero.Core.Entities;
using zero.Web.Models;

namespace zero.Web.Mapper
{
  public class CountryMapperConfig : IMapperConfig
  {
    /// <inheritdoc />
    public void Configure(IMapper config)
    {
      config.CreateMap<Country, CountryEditModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.IsActive = source.IsActive;
        target.CreatedDate = source.CreatedDate;
        target.IsPreferred = source.IsPreferred;
        target.LanguageId = source.LanguageId;
        target.Code = source.Code;
      });

      config.CreateMap<CountryEditModel, Country>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.IsActive = source.IsActive;
        target.CreatedDate = source.CreatedDate;
        target.IsPreferred = source.IsPreferred;
        target.LanguageId = source.LanguageId;
        target.Code = source.Code;
      });

      config.CreateMap<Country, CountryListModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.IsActive = source.IsActive;
        target.IsPreferred = source.IsPreferred;
        target.Code = source.Code;
      });
    }
  }
}
