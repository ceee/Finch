namespace zero.Api.Endpoints.Pages;

public class PageMapperProfile : ZeroMapperProfile
{
  public override void Configure(IZeroMapper mapper)
  {
    mapper.Define<Page, PageBasic>((source, ctx) => new(), Map);
   // mapper.Define<CountrySave, Country>((source, ctx) => new(), Map);
  }


  protected virtual void Map(Page source, PageBasic target, IZeroMapperContext ctx)
  {
    this.MapBasicData(source, target);
  }

  //protected virtual void Map(CountrySave source, Country target, IZeroMapperContext ctx)
  //{
  //  this.MapSaveData(source, target);
  //  target.Code = source.Code;
  //  target.IsPreferred = source.IsPreferred;
  //}
}