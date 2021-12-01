namespace zero.Api.Endpoints.Countries;

public class CountryMapperProfile : ZeroMapperProfile
{
  public override void Configure(IZeroMapper mapper)
  {
    mapper.Define<Country, CountryBasic>((source, ctx) => new(), Map);
    mapper.Define<Country, CountryEdit>((source, ctx) => new(), Map);
    mapper.Define<CountrySave, Country>((source, ctx) => new(), Map);
  }


  protected virtual void Map(Country source, CountryBasic target, IZeroMapperContext ctx)
  {
    this.MapBasicData(source, target);
    target.Code = source.Code;
    target.IsPreferred = source.IsPreferred;
  }

  protected virtual void Map(Country source, CountryEdit target, IZeroMapperContext ctx)
  {
    this.MapDisplayData(source, target);
    target.Code = source.Code;
    target.IsPreferred = source.IsPreferred;
  }

  protected virtual void Map(CountrySave source, Country target, IZeroMapperContext ctx)
  {
    this.MapSaveData(source, target);
    target.Code = source.Code;
    target.IsPreferred = source.IsPreferred;
  }
}