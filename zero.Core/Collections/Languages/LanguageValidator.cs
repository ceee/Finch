using FluentValidation;
using System;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Collections
{
  public class LanguageValidator : ZeroValidator<Language>
  {
    public LanguageValidator(IZeroDocumentSession session)
    {
      RuleFor(x => x.Name).Length(2, 60);
      RuleFor(x => x.Code).Length(2, 10).Culture();
      RuleFor(x => x.IsDefault).Unique(session).When(x => x.IsDefault).WithMessage("@language.errors.default_unique");
      RuleFor(x => x.IsDefault).ExpectAnyUnique(session, expectedValue: true).When(x => !x.IsDefault).WithMessage("@language.errors.needs_default");
      RuleFor(x => x.InheritedLanguageId).Must((entity, value) => !entity.Id.Equals(value, StringComparison.InvariantCultureIgnoreCase)).When(x => !x.Id.IsNullOrEmpty()).WithMessage("@language.errors.fallback_invalid");
      RuleFor(x => x.InheritedLanguageId).Equal((string)null).When(x => x.IsDefault).WithMessage("@language.errors.default_no_fallback");
      RuleFor(x => x.InheritedLanguageId).Exists(session);
      RuleFor(x => x.IsOptional).Equal(false).When(x => x.IsDefault).WithMessage("@language.errors.default_not_optional");
    }
  }
}
