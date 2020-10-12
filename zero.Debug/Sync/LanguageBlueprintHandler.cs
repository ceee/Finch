using Raven.Client.Documents.Session;
using zero.Core.Entities;

namespace zero.Debug.Sync
{
  public class LanguageBlueprintHandler : BlueprintHandler<ILanguage>
  {
    protected override void OnBlueprintCreate(ILanguage blueprint, ILanguage model)
    {
      model.Code = blueprint.Code;
      model.IsOptional = blueprint.IsOptional;
      model.IsDefault = blueprint.IsDefault;
      model.InheritedLanguageId = blueprint.InheritedLanguageId;
    }
  }
}
