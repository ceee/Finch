using FluentValidation;
using zero.Core.Entities;
using zero.Core.Entities.Setup;
using zero.Core.Extensions;

namespace zero.Core.Validation
{
  public class LanguageValidator : AbstractValidator<Language>
  {
    public LanguageValidator()
    {
      //RuleFor(x => x.Code).Length(2);
      //RuleFor(x => x.Name).Length(2, 120);
    }
  }
}
