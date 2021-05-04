using FluentValidation;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Collections
{
  public class TranslationValidator : ZeroValidator<Translation>
  {
    public TranslationValidator(IBackofficeStore store)
    {
      RuleFor(x => x.Key).Length(2, 300).Unique(store);
      RuleFor(x => x.Value).MaximumLength(10 * 1000);
    }
  }
}
