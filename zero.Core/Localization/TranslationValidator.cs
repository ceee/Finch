using FluentValidation;

namespace zero.Localization;

public class TranslationValidator : ZeroValidator<Translation>
{
  public TranslationValidator(IZeroStore store)
  {
    RuleFor(x => x.Key).NotEmpty().Length(2, 300).Unique(store);
    RuleFor(x => x.Value).MaximumLength(10 * 1000);
  }
}