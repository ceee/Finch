using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Security
{
  public class SchemedSignInManager<TUser> : SignInManager<TUser> where TUser : class, IIdentityUser
  {
    protected ZeroAuthOptions<TUser> AuthOptions { get; private set; }

    public SchemedSignInManager(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory,
      IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<TUser> confirmation, IOptions<ZeroAuthOptions<TUser>> authOptions)
      : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
      AuthOptions = authOptions.Value;
    }


    /// <inheritdoc />
    public override bool IsSignedIn(ClaimsPrincipal principal)
    {
      if (principal == null)
      {
        throw new ArgumentNullException(nameof(principal));
      }
      return principal?.Identities != null && principal.Identities.Any(i => i.AuthenticationType == AuthOptions.Scheme);
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
}
