using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace zero.Identity;

public class ZeroClaimsPrinicipalFactory<TUser, TRole> : ZeroClaimsPrinicipalFactory<TUser>
  where TUser : ZeroIdentityUser
  where TRole : ZeroIdentityRole
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
  where TUser : ZeroIdentityUser
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
    return userIdentity;
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

    return claims;
  }
}
