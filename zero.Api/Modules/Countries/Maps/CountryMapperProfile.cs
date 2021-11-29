namespace zero.Api.Modules.Countries;

public class CountryMapperProfile : ZeroMapperProfile
{
  public override void Configure(IZeroMapper mapper)
  {
    mapper.Define<Country, CountryBasic>((source, ctx) => new(), Map);
    mapper.Define<Country, CountryDisplay>((source, ctx) => new(), Map);
    mapper.Define<Country, PickerModel>((source, ctx) => new(), Map);
    mapper.Define<Country, PickerPreviewModel>((source, ctx) => new(), Map);
    mapper.Define<CountrySave, Country>((source, ctx) => new(), Map);
  }


  void Map(Country source, CountryBasic target, IZeroMapperContext ctx)
  {
    this.MapBasicData(source, target);
    target.Code = source.Code;
    target.IsPreferred = source.IsPreferred;
  }

  void Map(Country source, CountryDisplay target, IZeroMapperContext ctx)
  {
    this.MapDisplayData(source, target);
    target.Code = source.Code;
    target.IsPreferred = source.IsPreferred;
  }

  void Map(CountrySave source, Country target, IZeroMapperContext ctx)
  {
    this.MapSaveData(source, target);
    target.Code = source.Code;
    target.IsPreferred = source.IsPreferred;
  }

  void Map(Country source, PickerModel target, IZeroMapperContext ctx)
  {
    target.Id = source.Id;
    target.Name = source.Name;
    target.IsActive = source.IsActive;
  }

  void Map(Country source, PickerPreviewModel target, IZeroMapperContext ctx)
  {
    target.Id = source.Id;
    target.Name = source.Name;
    target.Icon = "flag-" + source.Code.ToLowerInvariant();
  }
}