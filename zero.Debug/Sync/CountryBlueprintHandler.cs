using Raven.Client.Documents.Session;
using zero.Core.Entities;

namespace zero.Debug.Sync
{
  public class CountryBlueprintHandler : BlueprintHandler<ICountry>
  {
    protected override void OnBlueprintCreate(ICountry blueprint, ICountry model)
    {
      model.Code = blueprint.Code;
      model.IsPreferred = blueprint.IsPreferred;
    }
  }
}
