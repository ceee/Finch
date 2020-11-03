using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Core.Security
{
  public class ZeroClaimsPrinicipalFactory<TUser, TRole> : UserClaimsPrincipalFactory<TUser, TRole>, IUserClaimsPrincipalFactory<TUser> where TUser : class, IUser where TRole : class, IUserRole
  {
    public ZeroClaimsPrinicipalFactory(UserManager<TUser> userManager, RoleManager<TRole> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
    {

    }

    public async override Task<ClaimsPrincipal> CreateAsync(TUser user)
    {
      if (user == null)
      {
        throw new ArgumentNullException(nameof(user));
      }

      ClaimsIdentity principal = await GenerateClaimsAsync(user);
      return new ClaimsPrincipal(principal);
    }


    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
    {
      string userId = await UserManager.GetUserIdAsync(user);
      string userName = await UserManager.GetUserNameAsync(user);

      List<Claim> claims = new List<Claim>();
      claims.Add(new Claim(Constants.Auth.Claims.IsZero, PermissionsValue.True));
      claims.Add(new Claim(Constants.Auth.Claims.UserId, userId));
      claims.Add(new Claim(Constants.Auth.Claims.UserName, userName));

      if (UserManager.SupportsUserSecurityStamp)
      {
        claims.Add(new Claim(Constants.Auth.Claims.SecurityStamp, await UserManager.GetSecurityStampAsync(user)));
      }

      if (UserManager.SupportsUserClaim)
      {
        claims.AddRange(await UserManager.GetClaimsAsync(user));
      }

      if (user.IsSuper)
      {
        claims.Add(new Claim(Constants.Auth.Claims.IsSuper, PermissionsValue.True));
      }


      // get all allowed app ids
      string[] appIds = claims
        .Where(x => x.Type == Constants.Auth.Claims.Permission && x.Value.StartsWith(Permissions.Applications))
        .Select(x => Permission.FromClaim(x, Permissions.Applications))
        .Where(x => x.IsTrue)
        .Select(x => x.NormalizedKey)
        .ToArray();

      string currentAppId = user.CurrentAppId ?? user.AppId;

      if (!user.IsSuper && !appIds.Contains(currentAppId))
      {
        currentAppId = user.AppId;
      }

      claims.Add(new Claim(Constants.Auth.Claims.CurrentAppId, currentAppId));

      // add default role when user has none
      if (!claims.Any(x => x.Type == Constants.Auth.Claims.Role))
      {
        claims.Add(new Claim(Constants.Auth.Claims.Role, "userRoles.1-A")); // TODO this needs to be dynamic
      }


      // create the user identity
      ClaimsIdentity identity = new ClaimsIdentity(claims, UserIdentity.Issuer, Constants.Auth.Claims.UserName, Constants.Auth.Claims.Role); // "Identity.Application"

      if (UserIdentity.TryCreate(identity, out UserIdentity userIdentity))
      {
        return userIdentity;
      }

      return null;
    }
  }
}
