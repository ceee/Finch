using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Core.Security
{
  public class ZeroClaimsPrinicipalFactory<TUser, TRole> : ZeroClaimsPrinicipalFactory<TUser>
    where TUser : class, IIdentityUserWithRoles, IIdentityUser
    where TRole : class, IIdentityUserRole
  {
    public ZeroClaimsPrinicipalFactory(UserManager<TUser> userManager, RoleManager<TRole> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
    { }
  }


  public class ZeroClaimsPrinicipalFactory<TUser> : UserClaimsPrincipalFactory<TUser>, IUserClaimsPrincipalFactory<TUser>
    where TUser : class, IIdentityUser
  {
    public ZeroClaimsPrinicipalFactory(UserManager<TUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
    { }

    public async override Task<ClaimsPrincipal> CreateAsync(TUser user)
    {
      if (user == null)
      {
        throw new ArgumentNullException(nameof(user));
      }

      ClaimsIdentity principal = await GenerateClaimsAsync(user);

      if (principal == null)
      {
        return null;
      }

      return new ClaimsPrincipal(principal);
    }


    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
    {
      List<Claim> claims = await CreateClaimList(user);

      // create the user identity
      ClaimsIdentity identity = new ClaimsIdentity(claims, UserIdentity.Issuer, Constants.Auth.Claims.UserName, Constants.Auth.Claims.Role); // "Identity.Application"

      if (UserIdentity.TryCreate(identity, out UserIdentity userIdentity))
      {
        return userIdentity;
      }

      return null;
    }


    protected virtual async Task<List<Claim>> CreateClaimList(TUser user)
    {
      string userId = await UserManager.GetUserIdAsync(user);
      string userName = await UserManager.GetUserNameAsync(user);
      string email = await UserManager.GetEmailAsync(user);

      List<Claim> claims = new List<Claim>();

      claims.Add(new Claim(Constants.Auth.Claims.IsZero, PermissionsValue.False));
      claims.Add(new Claim(Constants.Auth.Claims.UserId, userId));
      claims.Add(new Claim(Constants.Auth.Claims.UserName, userName));
      claims.Add(new Claim(Constants.Auth.Claims.Email, email));

      if (UserManager.SupportsUserSecurityStamp)
      {
        claims.Add(new Claim(Constants.Auth.Claims.SecurityStamp, await UserManager.GetSecurityStampAsync(user)));
      }

      if (UserManager.SupportsUserClaim)
      {
        claims.AddRange(await UserManager.GetClaimsAsync(user));
      }

      claims.Add(new Claim(Constants.Auth.Claims.AppId, user.AppId));

      return claims;
    }
  }
}
