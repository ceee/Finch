using FluentValidation;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Validation
{
  public class ApplicationValidator : ZeroValidator<IApplication>
  {
    public ApplicationValidator()
    {
      //RuleFor(x => x.Code).Length(2);
      RuleFor(x => x.Name).NotEmpty().Length(2, 50);
      RuleFor(x => x.FullName).MaximumLength(120);
      RuleFor(x => x.Email).Email().NotEmpty().MaximumLength(120);
      RuleFor(x => x.Domains).NotEmpty();
    }
  }
}
