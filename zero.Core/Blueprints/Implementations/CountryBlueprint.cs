using zero.Core.Entities;

namespace zero.Core.Blueprints
{
  public class CountryBlueprint : Blueprint<Country>
  {
    public CountryBlueprint()
    {
      Sync(x => x.Code);
      Sync(x => x.IsPreferred);
    }
  }
}