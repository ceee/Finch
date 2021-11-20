using FluentValidation;

namespace zero.Applications;

public class ApplicationValidator : ZeroValidator<Application>
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
