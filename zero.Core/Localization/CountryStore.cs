using FluentValidation;

namespace zero.Localization;

public class CountryStore : EntityStore<Country>, ICountryStore
{
  public CountryStore(IStoreContext context) : base(context) { }

  /// <inheritdoc />
  protected override void ValidationRules(ZeroValidator<Country> validator)
  {
    validator.RuleFor(x => x.Code).Length(2).Unique(Session);
    validator.RuleFor(x => x.Name).Length(2, 120);
  }
}


public interface ICountryStore : IEntityStore<Country> { }