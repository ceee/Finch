using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Security;

namespace zero.Core.Api
{
  public class AuthenticationApi : IAuthenticationApi
  {
    protected IDocumentStore Raven { get; private set; }

    protected IHttpContextAccessor HttpContextAccessor { get; set; }

    protected SignInManager<User> SignInManager { get; private set; }

    protected IUserClaimsPrincipalFactory<User> ClaimsPrincipalFactory { get; private set; }

    protected ClaimsPrincipal Principal => HttpContextAccessor.HttpContext?.User;


    public AuthenticationApi(IDocumentStore raven, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager, IUserClaimsPrincipalFactory<User> claimsPrincipalFactory)
    {
      Raven = raven;
      HttpContextAccessor = httpContextAccessor;
      SignInManager = signInManager;
      ClaimsPrincipalFactory = claimsPrincipalFactory;
    }


    /// <inheritdoc />
    public bool IsLoggedIn()
    {
      ClaimsPrincipal principal = HttpContextAccessor.HttpContext.User;

      bool isAuthenticated = principal.Identity.IsAuthenticated;
      bool isZeroUser = principal.HasClaim(Constants.Auth.Claims.IsZero, PermissionsValue.True);

      bool isSignedIn = SignInManager.IsSignedIn(principal);

      return isAuthenticated && isZeroUser && isSignedIn;
    }


    /// <inheritdoc />
    public bool IsSuper()
    {
      return Principal.HasClaim(Constants.Auth.Claims.IsSuper, PermissionsValue.True);
    }


    /// <inheritdoc />
    public bool IsAdmin()
    {
      return Principal.HasClaim(Constants.Auth.Claims.Role, "administrator"); // TODO use constant (in setup as well)
    }


    /// <inheritdoc />
    public async Task<User> GetUser()
    {
      //var userIdClaim = Principal?.FindFirst(ClaimTypes.NameIdentifier);

      return await SignInManager.UserManager.GetUserAsync(HttpContextAccessor.HttpContext.User);
    }


    /// <inheritdoc />
    public IList<Permission> GetPermissions(string prefix = null)
    {
      return Principal.Claims
        .Where(claim => claim.Type == Constants.Auth.Claims.Permission && (prefix == null || claim.Value.StartsWith(prefix)))
        .Select(claim => Permission.FromClaim(claim, prefix))
        .ToList();
    }


    /// <inheritdoc />
    public Permission GetPermission(string key = null)
    {
      Claim claim = Principal.Claims.FirstOrDefault(claim => claim.Type == Constants.Auth.Claims.Permission && claim.Value.StartsWith(key + ":"));

      return Permission.FromClaim(claim);
    }


    /// <inheritdoc />
    public async Task<EntityResult> Login(string email, string password, bool isPersistent)
    {
      EntityResult result = new EntityResult();

      User user = await SignInManager.UserManager.FindByNameAsync(email);

      if (user == null)
      {
        result.AddError("@login.errors.wrongcredentials");
        return result;
      }
      // TODO probably move this logic into a custom SignInManager which overrides CanSignInAsync()
      // see https://stackoverflow.com/a/35484758/670860
      else if (!user.IsActive)
      {
        result.AddError("@login.errors.disabled");
        return result;
      }



      SignInResult signInResult = await SignInManager.CheckPasswordSignInAsync(user, password, true);

      if (!signInResult.Succeeded)
      {
        if (signInResult.IsLockedOut)
        {
          result.AddError("@login.errors.lockedout");
        }
        else if (signInResult.IsNotAllowed)
        {
          result.AddError("@login.errors.notallowed");
        }
        else if (signInResult.RequiresTwoFactor)
        {
          result.AddError("@login.errors.requirestwofactor");
        }
        else
        {
          result.AddError("@login.errors.wrongcredentials");
        }

        return result;
      }



      ClaimsPrincipal userPrincipal = await ClaimsPrincipalFactory.CreateAsync(user);
      //var claims = new[] {new Claim(Constants.Auth.Claims.UserId, user.Id) };
      //var claimsIdentity = new ClaimsIdentity(claims, Constants.Auth.Scheme);
      //var userPrincipal = new ClaimsPrincipal(claimsIdentity);

      

      userPrincipal.Identities.FirstOrDefault()?.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, Constants.Auth.Scheme));

      await SignInManager.Context.SignInAsync(Constants.Auth.Scheme, userPrincipal, new AuthenticationProperties()
      {
        IsPersistent = isPersistent
      });

      var xuser = HttpContextAccessor.HttpContext.User;
      var xuserid = GetUserId();
      var yuser = await GetUser();

      return EntityResult.Success();
    }


    /// <inheritdoc />
    public async Task Logout()
    {
      await SignInManager.Context.SignOutAsync(Constants.Auth.Scheme);
    }


    /// <inheritdoc />
    public string GetUserId()
    {
      return HttpContextAccessor.HttpContext.User.FindFirstValue(Constants.Auth.Claims.UserId);
    }
  }


  public interface IAuthenticationApi
  {
    /// <summary>
    /// Get currently logged-in user
    /// </summary>
    Task<User> GetUser();

    /// <summary>
    /// Whether a user is currently logged-in
    /// </summary>
    bool IsLoggedIn();

    /// <summary>
    /// Whether the current user is the super user who created the zero instance
    /// </summary>
    bool IsSuper();

    /// <summary>
    /// Whether the current user belongs to the administrator role (will always return false if this role gets deleted)
    /// </summary>
    bool IsAdmin();

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

    /// <summary>
    /// Get all permissions for the current user with an optional prefix
    /// </summary>
    IList<Permission> GetPermissions(string prefix = null);

    /// <summary>
    /// Get a single permissions by key
    /// </summary>
    public Permission GetPermission(string key = null);
  }
}
