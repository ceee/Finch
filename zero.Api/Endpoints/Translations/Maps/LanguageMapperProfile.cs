namespace zero.Api.Endpoints.Translations;

public class TranslationMapperProfile : ZeroMapperProfile
{
  public override void Configure(IZeroMapper mapper)
  {
    mapper.Define<Translation, TranslationBasic>((source, ctx) => new(), Map);
    mapper.Define<Translation, TranslationEdit>((source, ctx) => new(), Map);
    mapper.Define<TranslationSave, Translation>((source, ctx) => new(), Map);
  }


  protected virtual void Map(Translation source, TranslationBasic target, IZeroMapperContext ctx)
  {
    this.MapBasicData(source, target);
    target.Value = source.Value;
    target.Display = source.Display;
  }

  protected virtual void Map(Translation source, TranslationEdit target, IZeroMapperContext ctx)
  {
    this.MapDisplayData(source, target);
    target.Value = source.Value;
    target.Display = source.Display;
  }

  protected virtual void Map(TranslationSave source, Translation target, IZeroMapperContext ctx)
  {
    this.MapSaveData(source, target);
    target.Value = source.Value;
    target.Display = source.Display;
  }
}