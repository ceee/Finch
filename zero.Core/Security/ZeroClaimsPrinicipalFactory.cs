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
    where TUser : class, IIdentityUserWithRoles
    where TRole : class, IIdentityUserRole
  {
    public RoleManager<TRole> RoleManager { get; private set; }
    
    public ZeroClaimsPrinicipalFactory(UserManager<TUser> userManager, RoleManager<TRole> roleManager, IOptions<IdentityOptions> optionsAccessor, IOptions<ZeroAuthOptions<TUser>> authOptions, IZeroContext zero) : base(userManager, optionsAccessor, authOptions, zero)
    {
      RoleManager = roleManager;
    }

    protected override async Task<List<Claim>> CreateClaimList(TUser user)
    {
      var claims = await base.CreateClaimList(user);

      if (UserManager.SupportsUserRole)
      {
        var roles = await UserManager.GetRolesAsync(user);

        foreach (var roleName in roles)
        {
          claims.Add(new Claim(Constants.Auth.Claims.Role, roleName));

          if (RoleManager.SupportsRoleClaims)
          {
            var role = await RoleManager.FindByNameAsync(roleName);
            if (role != null)
            {
              claims.AddRange(await RoleManager.GetClaimsAsync(role));
            }
          }
        }
      }

      return claims;
    }
  }


  public class ZeroClaimsPrinicipalFactory<TUser> : UserClaimsPrincipalFactory<TUser>, IUserClaimsPrincipalFactory<TUser>
    where TUser : class, IIdentityUser
  {
    protected ZeroAuthOptions<TUser> AuthOptions { get; private set; }

    protected IZeroContext Zero { get; private set; }


    public ZeroClaimsPrinicipalFactory(UserManager<TUser> userManager, IOptions<IdentityOptions> optionsAccessor, IOptions<ZeroAuthOptions<TUser>> authOptions, IZeroContext zero) : base(userManager, optionsAccessor)
    {
      AuthOptions = authOptions.Value;
      Zero = zero;
    }


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
      ClaimsIdentity identity = new ClaimsIdentity(claims, AuthOptions.Scheme, Constants.Auth.Claims.UserName, Constants.Auth.Claims.Role); // "Identity.Application"

      bool isUserIdentity = UserIdentity.TryCreate(identity, AuthOptions.Scheme, out UserIdentity userIdentity);
      
      if (isUserIdentity)
      {
        Claim isZeroClaim = userIdentity.FindFirst(Constants.Auth.Claims.IsZero);
        Claim appIdClaim = userIdentity.FindFirst(Constants.Auth.Claims.AppId);
        string appId = appIdClaim?.Value;

        if (appIdClaim is null || isZeroClaim is null or not { Value: PermissionsValue.False })
        {
          return null;
        }

        if (appId is not null || Zero.AppId.Equals(appId, StringComparison.InvariantCultureIgnoreCase))
        {
          return userIdentity;
        }
      }

      return null;
    }


    protected virtual async Task<List<Claim>> CreateClaimList(TUser user)
    {
      string userId = await UserManager.GetUserIdAsync(user);
      string userName = await UserManager.GetUserNameAsync(user);

      List<Claim> claims = new List<Claim>();

      claims.Add(new Claim(Constants.Auth.Claims.IsZero, PermissionsValue.False));
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

      if (UserManager.SupportsUserEmail)
      {
        claims.Add(new Claim(Constants.Auth.Claims.Email, await UserManager.GetEmailAsync(user)));
      }

      //claims.Add(new Claim(Constants.Auth.Claims.AppId, user.AppId));  // TODO appx fix

      return claims;
    }
  }
}
