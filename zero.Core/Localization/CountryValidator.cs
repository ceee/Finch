using FluentValidation;

namespace zero.Localization;

public class CountryValidator : ZeroValidator<Country>
{
  public CountryValidator(IZeroStore store)
  {
    RuleFor(x => x.Code).NotEmpty().Length(2).Unique(store);
    RuleFor(x => x.Name).NotEmpty().Length(2, 120);
  }
}