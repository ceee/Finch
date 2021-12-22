//using zero.Mapper;
//using zero.Spaces;

using zero.Localization;

namespace zero.Demo;

//public class Mapper : ZeroMapperProfile
//{
//  public override void Configure(IZeroMapper mapper)
//  {
//    mapper.Define<TeamMember, LanguageBasic>((source, ctx) => new(), Map);
//    mapper.Define<Language, LanguageEdit>((source, ctx) => new(), Map);
//    mapper.Define<LanguageSave, Language>((source, ctx) => new(), Map);
//  }


//  protected virtual void Map(Language source, LanguageBasic target, IZeroMapperContext ctx)
//  {
//    this.MapBasicData(source, target);
//    target.Code = source.Code;
//    target.IsDefault = source.IsDefault;
//    target.InheritedLanguageId = source.InheritedLanguageId;
//  }
//}


public class EuropeanCountry : Country
{
  public string EuName { get; set; }
}

public class AmericanCountry : Country
{
  public string State { get; set; }
}