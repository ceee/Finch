using FluentValidation;

namespace zero.Localization;

public class TranslationValidator : ZeroValidator<Translation>
{
  public TranslationValidator(ZeroValidationContext ctx)
  {
    RuleFor(x => x.Key).NotEmpty().Length(2, 300).Unique(ctx);
    RuleFor(x => x.Value).MaximumLength(10 * 1000);
  }
}