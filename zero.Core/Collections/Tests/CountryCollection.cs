namespace zero.Core;

using FluentValidation;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

public class CountryCollection : EntityCollection<Country>, ICountryCollection
{
  public CountryCollection() : base() { }


  /// <inheritdoc />
  protected override void ValidationRules(ZeroValidator<Country> validator)
  {
    validator.RuleFor(x => x.Code).Length(2).Unique(Session);
    validator.RuleFor(x => x.Name).Length(2, 120);
  }
}

public interface ICountryCollection : IEntityCollection<Country> { }