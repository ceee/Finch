namespace zero.Api.Endpoints.Languages;

public class LanguageMapperProfile : ZeroMapperProfile
{
  public override void Configure(IZeroMapper mapper)
  {
    mapper.Define<Language, LanguageBasic>((source, ctx) => new(), Map);
    mapper.Define<Language, LanguageEdit>((source, ctx) => new(), Map);
    mapper.Define<LanguageSave, Language>((source, ctx) => new(), Map);
  }


  protected virtual void Map(Language source, LanguageBasic target, IZeroMapperContext ctx)
  {
    this.MapBasicData(source, target);
    target.Code = source.Code;
    target.IsDefault = source.IsDefault;
    target.InheritedLanguageId = source.InheritedLanguageId;
  }

  protected virtual void Map(Language source, LanguageEdit target, IZeroMapperContext ctx)
  {
    this.MapDisplayData(source, target);
    target.Code = source.Code;
    target.IsDefault = source.IsDefault;
    target.InheritedLanguageId = source.InheritedLanguageId;
  }

  protected virtual void Map(LanguageSave source, Language target, IZeroMapperContext ctx)
  {
    this.MapSaveData(source, target);
    target.Code = source.Code;
    target.IsDefault = source.IsDefault;
    target.InheritedLanguageId = source.InheritedLanguageId;
  }
}