using FluentValidation;
using zero.Core.Entities;
using zero.Core.Entities.Setup;
using zero.Core.Extensions;

namespace zero.Core.Validation
{
  public class ApplicationValidator : AbstractValidator<IApplication>
  {
    public ApplicationValidator()
    {
      //RuleFor(x => x.Code).Length(2);
      RuleFor(x => x.Name).NotEmpty().Length(2, 120);
      RuleFor(x => x.Domains).NotEmpty();
    }
  }
}
