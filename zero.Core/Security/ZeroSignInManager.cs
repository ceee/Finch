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
using zero.Core.Identity;

namespace zero.Core.Security
{
  public class ZeroSignInManager<TUser> : SignInManager<TUser> where TUser : class, IIdentityUser
  {
    public ZeroSignInManager(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory,
      IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<TUser> confirmation)
      : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation) { }


    public override bool IsSignedIn(ClaimsPrincipal principal)
    {
      if (principal == null)
      {
        throw new ArgumentNullException(nameof(principal));
      }
      return principal?.Identities != null && principal.Identities.Any(i => i.AuthenticationType == Constants.Auth.BackofficeScheme && i.HasClaim(Constants.Auth.Claims.IsZero, PermissionsValue.True));
    }


    public override async Task RefreshSignInAsync(TUser user)
    {
      var auth = await Context.AuthenticateAsync(Constants.Auth.BackofficeScheme);
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

    public override async Task SignInWithClaimsAsync(TUser user, AuthenticationProperties authenticationProperties, IEnumerable<Claim> additionalClaims)
    {
      var userPrincipal = await CreateUserPrincipalAsync(user);

      foreach (var claim in additionalClaims)
      {
        userPrincipal.Identities.First().AddClaim(claim);
      }
      await Context.SignInAsync(Constants.Auth.BackofficeScheme, userPrincipal, authenticationProperties ?? new AuthenticationProperties());
    }


    public override async Task SignOutAsync()
    {
      await Context.SignOutAsync(Constants.Auth.BackofficeScheme);
    }
  }
}
