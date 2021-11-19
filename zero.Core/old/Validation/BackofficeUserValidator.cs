using FluentValidation;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Validation
{
  public class BackofficeUserValidator : ZeroValidator<BackofficeUser>
  {
    public BackofficeUserValidator(bool isCreate = false)
    {
      RuleFor(x => x.Name).Length(2, 80);
      RuleFor(x => x.Email).NotEmpty().Email().MaximumLength(120);
      RuleFor(x => x.PasswordHash).NotEmpty();
      RuleFor(x => x.LanguageId).NotEmpty();
      RuleFor(x => x.RoleIds).NotEmpty();

      if (isCreate)
      {
        RuleFor(x => x.IsSuper).Equals(false);
      }
    }
  }
}
