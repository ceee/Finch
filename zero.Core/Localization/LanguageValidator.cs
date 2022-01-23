using FluentValidation;

namespace zero.Localization;

public class LanguageValidator : ZeroValidator<Language>
{
  public LanguageValidator(ZeroValidationContext ctx)
  {
    RuleFor(x => x.Name).NotEmpty().Length(2, 60);
    RuleFor(x => x.Code).NotEmpty().Length(2, 10).Culture();
    RuleFor(x => x.IsDefault).Unique(ctx).When(x => x.IsDefault).WithMessage("@language.errors.default_unique");
    RuleFor(x => x.IsDefault).ExpectAnyUnique(ctx, expectedValue: true).When(x => !x.IsDefault).WithMessage("@language.errors.needs_default");
    RuleFor(x => x.InheritedLanguageId).Must((entity, value) => !entity.Id.Equals(value, StringComparison.InvariantCultureIgnoreCase)).When(x => !x.Id.IsNullOrEmpty()).WithMessage("@language.errors.fallback_invalid");
    RuleFor(x => x.InheritedLanguageId).Equal((string)null).When(x => x.IsDefault).WithMessage("@language.errors.default_no_fallback");
    RuleFor(x => x.InheritedLanguageId).Exists(ctx);
    RuleFor(x => x.IsOptional).Equal(false).When(x => x.IsDefault).WithMessage("@language.errors.default_not_optional");
  }
}