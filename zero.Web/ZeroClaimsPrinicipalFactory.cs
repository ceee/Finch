using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web
{
  public class ZeroClaimsPrinicipalFactory : UserClaimsPrincipalFactory<User, UserRole>
  {
    public ZeroClaimsPrinicipalFactory(UserManager<User> userManager, RoleManager<UserRole> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
    {

    }

    public async override Task<ClaimsPrincipal> CreateAsync(User user)
    {
      ClaimsPrincipal principal = await base.CreateAsync(user);
      ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;

      identity.AddClaim(new Claim(Constants.Auth.Claims.IsZero, PermissionsValue.True));

      if (user.IsSuper)
      {
        identity.AddClaim(new Claim(Constants.Auth.Claims.IsSuper, PermissionsValue.True));
      }

      return principal;
    }
  }
}
