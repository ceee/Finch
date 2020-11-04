using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Options;

namespace zero.Core.Security
{
  public class ZeroCookieAuthenticationEvents<TUser> : CookieAuthenticationEvents where TUser : class, IIdentityUser
  {
    protected IZeroOptions Zero { get; set; }

    protected ISystemClock SystemClock { get; set; }


    public ZeroCookieAuthenticationEvents(IZeroOptions zero, ISystemClock systemClock)
    {
      Zero = zero;
      SystemClock = systemClock;

      //OnValidatePrincipal = _ValidatePrincipal;
      //OnSigningIn = _OnSigningIn;
      //OnSignedIn = _OnSignedIn;
      OnSigningOut = _OnSigningOut;
      OnRedirectToLogin = _OnRedirectToLogin;
    }


    /// <summary>
    /// Validate user session
    /// </summary>
    async Task _ValidatePrincipal(CookieValidatePrincipalContext ctx)
    {
      ISecurityStampValidator securityStampValidator = ctx.HttpContext.RequestServices.GetRequiredService<ISecurityStampValidator>();
      SignInManager<TUser> signInManager = ctx.HttpContext.RequestServices.GetRequiredService<SignInManager<TUser>>();

      UserIdentity identity = GetIdentity(ctx.Principal);

      if (identity == null)
      {
        ctx.RejectPrincipal();
        await signInManager.Context.SignOutAsync(Constants.Auth.BackofficeScheme);
        return;
      }

      // ensure the thread culture is set
      //backOfficeIdentity.EnsureCulture();
      //await EnsureValidSessionId(ctx);

      await securityStampValidator.ValidateAsync(ctx);

      EnsureTicketRenewalIfKeepUserLoggedIn(ctx);

      // add a claim to track when the cookie expires, we use this to track time remaining
      identity.AddClaim(new Claim(Constants.Auth.Claims.TicketExpires, ctx.Properties.ExpiresUtc.Value.ToString("o"), ClaimValueTypes.DateTime, UserIdentity.Issuer, UserIdentity.Issuer, identity));
    }


    /// <summary>
    /// 
    /// </summary>
    Task _OnSigningIn(CookieSigningInContext ctx)
    {
      UserIdentity identity = GetIdentity(ctx.Principal);

      if (identity != null)
      {
        identity.AddClaim(new Claim(ClaimTypes.CookiePath, "/", ClaimValueTypes.String, UserIdentity.Issuer, UserIdentity.Issuer, identity));
      }

      return Task.CompletedTask;
    }


    /// <summary>
    /// 
    /// </summary>
    Task _OnSignedIn(CookieSignedInContext ctx)
    {
      ctx.HttpContext.User = ctx.Principal;
      return Task.CompletedTask;
    }


    /// <summary>
    /// 
    /// </summary>
    Task _OnSigningOut(CookieSigningOutContext ctx)
    {
      ctx.Options.CookieManager.DeleteCookie(ctx.HttpContext, Constants.Auth.BackofficeCookieName, new CookieOptions() { Path = "/" });
      return Task.CompletedTask;
    }


    /// <summary>
    /// 
    /// </summary>
    Task _OnRedirectToLogin(RedirectContext<CookieAuthenticationOptions> ctx)
    {
      ctx.Response.StatusCode = 401;
      return Task.CompletedTask;
    }


    /// <summary>
    /// Build backoffice user identity from a matching principal identity
    /// </summary>
    UserIdentity GetIdentity(IPrincipal principal)
    {
      if (principal.Identity is UserIdentity user)
      {
        return user;
      }

      if (principal is ClaimsPrincipal claimsPrincipal)
      {
        user = claimsPrincipal.Identities.OfType<UserIdentity>().FirstOrDefault();
        if (user != null)
        {
          return user;
        }
      }

      if (principal.Identity is ClaimsIdentity claimsIdentity && claimsIdentity.IsAuthenticated)
      {
        if (UserIdentity.TryCreate(claimsIdentity, out user))
        {
          return user;
        }
      }

      return null;
    }


    /// <summary>
    /// Ensures the ticket is renewed if the <see cref="SecuritySettings.KeepUserLoggedIn"/> is set to true and the current request is for the get user seconds endpoint
    /// </summary>
    void EnsureTicketRenewalIfKeepUserLoggedIn(CookieValidatePrincipalContext context)
    {
      //if (!Zero.KeepUserLoggedIn) return; // TODO

      var currentUtc = SystemClock.UtcNow;
      var issuedUtc = context.Properties.IssuedUtc;
      var expiresUtc = context.Properties.ExpiresUtc;

      if (expiresUtc.HasValue && issuedUtc.HasValue)
      {
        var timeElapsed = currentUtc.Subtract(issuedUtc.Value);
        var timeRemaining = expiresUtc.Value.Subtract(currentUtc);

        if (timeRemaining < timeElapsed)
        {
          context.ShouldRenew = true;
        }
      }
    }
  }
}
