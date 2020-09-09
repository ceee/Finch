using FluentValidation;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Validation
{
  public class TranslationValidator : ZeroValidator<ITranslation>
  {
    public TranslationValidator(IBackofficeStore store)
    {
      RuleFor(x => x.Key).Length(2, 300).Unique(store);
      RuleFor(x => x.Value).MaximumLength(10 * 1000);
    }
  }
}
