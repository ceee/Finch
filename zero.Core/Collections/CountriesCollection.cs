using FluentValidation;

namespace zero.Collections;

public class CountriesCollection : CachableEntityCollection<Country>, ICountriesCollection
{
  public CountriesCollection(ICollectionContext context, ICollectionCache cache) : base(context, cache) { }

  /// <inheritdoc />
  protected override void ValidationRules(ZeroValidator<Country> validator)
  {
    validator.RuleFor(x => x.Code).Length(2).Unique(Session);
    validator.RuleFor(x => x.Name).Length(2, 120);
  }
}


public interface ICountriesCollection : IEntityCollection<Country> { }