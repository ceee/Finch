using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Core.Security
{
  public class ZeroBackofficeClaimsPrincipalFactory<TUser, TRole> : ZeroClaimsPrinicipalFactory<TUser, TRole>
    where TUser : class, IBackofficeUser
    where TRole : class, IBackofficeUserRole
  {
    public ZeroBackofficeClaimsPrincipalFactory(UserManager<TUser> userManager, RoleManager<TRole> roleManager, IOptions<IdentityOptions> optionsAccessor, IOptions<ZeroAuthOptions<TUser>> authOptions, IZeroContext zero) 
      : base(userManager, roleManager, optionsAccessor, authOptions, zero)
    { }


    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
    {
      List<Claim> claims = await CreateClaimList(user);

      // create the user identity
      ClaimsIdentity identity = new ClaimsIdentity(claims, AuthOptions.Scheme, Constants.Auth.Claims.UserName, Constants.Auth.Claims.Role); // "Identity.Application"

      if (BackofficeUserIdentity.TryCreate(identity, out BackofficeUserIdentity userIdentity))
      {
        return userIdentity;
      }

      return null;
    }


    protected override async Task<List<Claim>> CreateClaimList(TUser user)
    {
      List<Claim> claims = await base.CreateClaimList(user);

      claims.RemoveAll(x => x.Type == Constants.Auth.Claims.IsZero);
      claims.Add(new Claim(Constants.Auth.Claims.IsZero, PermissionsValue.True));

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

      claims.RemoveAll(x => x.Type == Constants.Auth.Claims.AppId);
      claims.Add(new Claim(Constants.Auth.Claims.AppId, currentAppId));
      claims.Add(new Claim(Constants.Auth.Claims.DefaultAppId, user.AppId));

      return claims;
    }
  }
}
