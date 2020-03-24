using FluentValidation;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Validation
{
  public class BackofficeUserValidator : AbstractValidator<IBackofficeUser>
  {
    public BackofficeUserValidator(bool isCreate = false)
    {
      RuleFor(x => x.Name).NotEmpty();
      RuleFor(x => x.Email).Email();
      RuleFor(x => x.PasswordHash).NotEmpty();
      RuleFor(x => x.LanguageId).NotEmpty(); // TODO only allow available languages
      RuleFor(x => x.RoleIds).NotEmpty();

      if (isCreate)
      {
        RuleFor(x => x.IsSuper).Equals(false);
      }
    }
  }
}
