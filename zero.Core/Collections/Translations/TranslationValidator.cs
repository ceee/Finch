using FluentValidation;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Collections
{
  public class TranslationValidator : ZeroValidator<Translation>
  {
    public TranslationValidator(IZeroDocumentSession session)
    {
      RuleFor(x => x.Key).Length(2, 300).Unique(session);
      RuleFor(x => x.Value).MaximumLength(10 * 1000);
    }
  }
}
