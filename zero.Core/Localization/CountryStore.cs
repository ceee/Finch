namespace zero.Localization;

public class CountryStore : EntityStore<Country>, ICountryStore
{
  public CountryStore(IStoreContext context) : base(context) { }
}


public interface ICountryStore : IEntityStore<Country> { }