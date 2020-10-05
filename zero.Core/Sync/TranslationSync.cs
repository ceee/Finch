using zero.Core.Entities;

namespace zero.Core.Sync
{
  public class TranslationSync
  {
    public void Map(ITranslation blueprint, ITranslation model)
    {
      model.Key = blueprint.Key;
      model.Value = blueprint.Value;
      model.Display = blueprint.Display;
    }
  }
}
