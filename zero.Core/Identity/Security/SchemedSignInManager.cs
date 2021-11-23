using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace zero.Identity;

// TODO although the login per application works
// the authentication breaks when another application signs in a user
// they share one cookie on both sites/applications, maybe this is an issue

public class SchemedSignInManager<TUser> : SignInManager<TUser> where TUser : ZeroIdentityUser
{
  protected ZeroAuthOptions<TUser> AuthOptions { get; private set; }

  protected IZeroContext Zero { get; private set; }

  public SchemedSignInManager(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory,
    IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<TUser> confirmation, IOptions<ZeroAuthOptions<TUser>> authOptions,
    IZeroContext zero)
    : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
  {
    AuthOptions = authOptions.Value;
    Zero = zero;
  }


  /// <inheritdoc />
  public override bool IsSignedIn(ClaimsPrincipal principal)
  {
    if (principal?.Identities == null)
    {
      return false;
    }
    if (!principal.Identities.Any(x => x.AuthenticationType == AuthOptions.Scheme))
    {
      return false;
    }

    string userAppId = principal.FindFirstValue(Constants.Auth.Claims.AppId);
    return userAppId == null || Zero.AppId.Equals(userAppId, StringComparison.InvariantCultureIgnoreCase);
  }


  /// <inheritdoc />
  public override async Task RefreshSignInAsync(TUser user)
  {
    var auth = await Context.AuthenticateAsync(AuthOptions.Scheme);
    IList<Claim> claims = Array.Empty<Claim>();

    var authenticationMethod = auth?.Principal?.FindFirst(ClaimTypes.AuthenticationMethod);
    var amr = auth?.Principal?.FindFirst("amr");

    if (authenticationMethod != null || amr != null)
    {
      claims = new List<Claim>();
      if (authenticationMethod != null)
      {
        claims.Add(authenticationMethod);
      }
      if (amr != null)
      {
        claims.Add(amr);
      }
    }

    await SignInWithClaimsAsync(user, auth?.Properties, claims);
  }


  /// <inheritdoc />
  public override async Task SignInWithClaimsAsync(TUser user, AuthenticationProperties authenticationProperties, IEnumerable<Claim> additionalClaims)
  {
    var userPrincipal = await CreateUserPrincipalAsync(user);
    foreach (var claim in additionalClaims)
    {
      userPrincipal.Identities.First().AddClaim(claim);
    }
    await Context.SignInAsync(AuthOptions.Scheme, userPrincipal, authenticationProperties ?? new AuthenticationProperties());
  }


  /// <inheritdoc />
  public override async Task SignOutAsync()
  {
    await Context.SignOutAsync(AuthOptions.Scheme);
  }
}
