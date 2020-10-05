using zero.Core.Entities;

namespace zero.Core.Sync
{
  public class CountrySync
  {
    public void Map(ICountry blueprint, ICountry model)
    {
      model.Code = blueprint.Code;
      model.IsPreferred = blueprint.IsPreferred;
    }
  }
}
