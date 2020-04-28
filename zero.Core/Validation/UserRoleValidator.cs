using FluentValidation;
using zero.Core.Entities;
using zero.Core.Extensions;
using System.Linq;
using System;

namespace zero.Core.Validation
{
  public class UserRoleValidator : AbstractValidator<UserRole>
  {
    const string SECTION_CLAIM = "section.";

    const string TRUE_CLAIM_VALUE = ":true";

    public UserRoleValidator()
    {
      RuleFor(x => x.Name).Length(2, 80);
      RuleFor(x => x.Description).MaximumLength(200);
      RuleFor(x => x.Icon).NotEmpty();

      RuleFor(x => x.Claims).NotEmpty().Must((role, claims, context) =>
      {
        foreach (IUserClaim claim in claims)
        {
          if (claim.Value.StartsWith(SECTION_CLAIM, StringComparison.InvariantCultureIgnoreCase) && claim.Value.EndsWith(TRUE_CLAIM_VALUE, StringComparison.InvariantCultureIgnoreCase))
          {
            return true;
          }
        }

        return false;
      }).WithMessage("@errors.role.nosection");

      RuleForEach(x => x.Claims).Must((role, claim, context) =>
      {
        return !claim.Type.IsNullOrEmpty() && !claim.Value.IsNullOrEmpty();
      }).WithMessage("@errors.role.emptyclaim");
    }
  }
}
