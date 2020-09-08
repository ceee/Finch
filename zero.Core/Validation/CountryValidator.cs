using FluentValidation;
using zero.Core.Entities;

namespace zero.Core.Validation
{
  public class CountryValidator : ZeroValidator<ICountry, Country>
  {
    public CountryValidator()
    {
      RuleFor(x => x.Code).Length(2);
      RuleFor(x => x.Name).Length(2, 120);
    }
  }
}
