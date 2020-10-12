using FluentValidation;
using System;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Validation
{
  public class LanguageValidator : ZeroValidator<ILanguage>
  {
    public LanguageValidator(IBackofficeStore store)
    {
      RuleFor(x => x.Name).Length(2, 60);
      RuleFor(x => x.Code).Length(2, 10).Culture();
      RuleFor(x => x.IsDefault).Unique(store).When(x => x.IsDefault).WithMessage("@language.errors.default_unique");
      RuleFor(x => x.IsDefault).ExpectAnyUnique(store, expectedValue: true).When(x => !x.IsDefault).WithMessage("@language.errors.needs_default");
      RuleFor(x => x.InheritedLanguageId).Must((entity, value) => !entity.Id.Equals(value, StringComparison.InvariantCultureIgnoreCase)).When(x => !x.Id.IsNullOrEmpty()).WithMessage("@language.errors.fallback_invalid");
      RuleFor(x => x.InheritedLanguageId).Equal((Ref<ILanguage>)null).When(x => x.IsDefault).WithMessage("@language.errors.default_no_fallback");
      RuleFor(x => x.InheritedLanguageId).Exists(store);
      RuleFor(x => x.IsOptional).Equal(false).When(x => x.IsDefault).WithMessage("@language.errors.default_not_optional");
    }
  }
}
