using FluentValidation;
using zero.Core.Entities.Setup;
using zero.Core.Extensions;

namespace zero.Core.Validation
{
  public class SetupModelValidator : ZeroValidator<SetupModel>
  {
    public SetupModelValidator()
    {
      RuleFor(x => x.User).NotNull();
      RuleFor(x => x.User.Email).NotEmpty().Email();
      RuleFor(x => x.User.Name).MaximumLength(40).NotEmpty();
      RuleFor(x => x.User.Password).MaximumLength(1024); // TODO password policy

      RuleFor(x => x.AppName).MaximumLength(40).NotEmpty();

      RuleFor(x => x.Database.Url).NotEmpty().Url();
      RuleFor(x => x.Database.Name).NotEmpty();
    }
  }
}
