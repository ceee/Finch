using FluentValidation;

namespace zero.Identity;

public class BackofficeUserRoleValidator : ZeroValidator<ZeroUserRole>
{
  const string SECTION_CLAIM = "section.";

  const string TRUE_CLAIM_VALUE = ":true";

  public BackofficeUserRoleValidator()
  {
    RuleFor(x => x.Name).Length(2, 80);
    RuleFor(x => x.Description).MaximumLength(200);
    RuleFor(x => x.Icon).NotEmpty();

    RuleFor(x => x.Claims).NotEmpty().Must((role, claims, context) =>
    {
      foreach (UserClaim claim in claims)
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
