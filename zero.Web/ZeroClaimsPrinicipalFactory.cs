using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
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

      // get all allowed app ids
      IEnumerable<Permission> permissions = identity.Claims
        .Where(claim => claim.Type == Constants.Auth.Claims.Permission && claim.Value.StartsWith(Permissions.Applications))
        .Select(x => Permission.FromClaim(x, Permissions.Applications));

      string[] appIds = permissions.Where(x => x.IsTrue).Select(x => x.NormalizedKey).ToArray();

      string currentAppId = user.CurrentAppId ?? user.AppId;

      if (!user.IsSuper && !appIds.Contains(currentAppId))
      {
        currentAppId = user.AppId;
      }

      identity.AddClaim(new Claim(Constants.Auth.Claims.CurrentAppId, currentAppId));

      return principal;
    }
  }
}
