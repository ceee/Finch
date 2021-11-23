using FluentValidation;

namespace zero.Stores;

public class CountriesStore : CachableEntityStore<Country>, ICountriesStore
{
  public CountriesStore(IStoreContext context, IStoreCache cache) : base(context, cache) { }

  /// <inheritdoc />
  protected override void ValidationRules(ZeroValidator<Country> validator)
  {
    validator.RuleFor(x => x.Code).Length(2).Unique(Session);
    validator.RuleFor(x => x.Name).Length(2, 120);
  }
}


public interface ICountriesStore : IEntityStore<Country> { }