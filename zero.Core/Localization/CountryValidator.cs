using FluentValidation;

namespace zero.Localization;

public class CountryValidator : ZeroValidator<Country>
{
  public CountryValidator(ZeroValidationContext ctx)
  {
    RuleFor(x => x.Code).NotEmpty().Length(2).Unique(ctx);
    RuleFor(x => x.Name).NotEmpty().Length(2, 120);
  }
}