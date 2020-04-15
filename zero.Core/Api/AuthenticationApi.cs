using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Api
{
  public class AuthenticationApi : IAuthenticationApi
  {
    protected IDocumentStore Raven { get; private set; }

    protected IHttpContextAccessor HttpContextAccessor { get; set; }

    protected SignInManager<User> SignInManager { get; private set; }


    public AuthenticationApi(IDocumentStore raven, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager)
    {
      Raven = raven;
      HttpContextAccessor = httpContextAccessor;
      SignInManager = signInManager;
    }


    /// <inheritdoc />
    public bool IsLoggedIn()
    {
      ClaimsPrincipal principal = HttpContextAccessor.HttpContext.User;

      bool isAuthenticated = principal.Identity.IsAuthenticated;
      bool isZeroUser = principal.HasClaim(Constants.Auth.Claims.IsZero, Constants.Auth.Claims.IsZero);

      return isAuthenticated && isZeroUser;
    }


    /// <inheritdoc />
    public async Task<EntityResult> Login(string email, string password, bool isPersistent)
    {
      SignInResult result = await SignInManager.PasswordSignInAsync(email, password, isPersistent, true);

      if (!result.Succeeded)
      {
        EntityResult failed = new EntityResult();
        
        if (result.IsLockedOut)
        {
          failed.AddError("@login.errors.lockedout");
        }
        else if (result.IsNotAllowed)
        {
          failed.AddError("@login.errors.notallowed");
        }
        else if (result.RequiresTwoFactor)
        {
          failed.AddError("@login.errors.requirestwofactor");
        }
        else
        {
          failed.AddError("@login.errors.wrongcredentials");
        }

        return failed;
      }

      return EntityResult.Success();
    }


    /// <inheritdoc />
    public async Task Logout()
    {
      await SignInManager.SignOutAsync();
    }


    /// <inheritdoc />
    public string GetUserId()
    {
      ClaimsPrincipal principal = HttpContextAccessor.HttpContext.User;
      return principal.Claims.FirstOrDefault(x => x.Type == Constants.Auth.Claims.UserId)?.Value;
    }
  }


  public interface IAuthenticationApi
  {
    /// <summary>
    /// Whether a user is currently logged-in
    /// </summary>
    bool IsLoggedIn();

    /// <summary>
    /// Logs a zero-user in and sets cookie
    /// </summary>
    Task<EntityResult> Login(string email, string password, bool isPersistent);

    /// <summary>
    /// Logs out the current user
    /// </summary>
    Task Logout();

    /// <summary>
    /// Get the ID of the currently logged in user
    /// </summary>
    string GetUserId();
  }
}
