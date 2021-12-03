using Microsoft.AspNetCore.Identity;
using zero.Identity.Models;

namespace zero.Identity;

public class AuthenticationService : IAuthenticationService
{
  protected IZeroContext Context { get; set; }

  protected SignInManager<ZeroUser> SignInManager { get; private set; }

  protected IZeroStore Store { get; set; }


  public AuthenticationService(IZeroContext context, SignInManager<ZeroUser> signInManager, IZeroStore store)
  {
    Context = context;
    SignInManager = signInManager;
    Store = store;
  }


  /// <inheritdoc />
  public bool IsLoggedIn()
  {
    //ClaimsPrincipal principal = HttpContextAccessor.HttpContext.User;
    //bool isAuthenticated = principal.Identity.IsAuthenticated;
    //bool isZeroUser = principal.HasClaim(Constants.Auth.Claims.IsZero, PermissionsValue.True);
    //bool isSignedIn = SignInManager.IsSignedIn(principal);
    //return isAuthenticated && isZeroUser && isSignedIn;

    return SignInManager.IsSignedIn(Context.BackofficeUser);
  }


  /// <inheritdoc />
  public bool IsSuper()
  {
    return Context.BackofficeUser.HasClaim(Constants.Auth.Claims.IsSuper, PermissionsValue.True);
  }


  /// <inheritdoc />
  public bool IsAdmin()
  {
    return Context.BackofficeUser.HasClaim(Constants.Auth.Claims.Role, "administrator"); // TODO use constant (in setup as well)
  }


  /// <inheritdoc />
  public async Task<ZeroUser> GetUser()
  {
    return await SignInManager.UserManager.GetUserAsync(Context.BackofficeUser);
  }


  /// <inheritdoc />
  public async Task<LoginResult> Login(string email, string password, bool isPersistent)
  {
    SignInResult signInResult = await SignInManager.PasswordSignInAsync(email, password, isPersistent, true);

    if (!signInResult.Succeeded)
    {
      if (signInResult.IsLockedOut)
      {
        return LoginResult.LockedOut;
      }
      else if (signInResult.IsNotAllowed)
      {
        return LoginResult.NotAllowed;
      }
      else if (signInResult.RequiresTwoFactor)
      {
        return LoginResult.RequiresTwoFactor;
      }
      
      return LoginResult.WrongCredentials;
    }

    return LoginResult.Success;
  }


  /// <inheritdoc />
  public async Task Logout()
  {
    await SignInManager.SignOutAsync();
  }


  /// <inheritdoc />
  public string GetUserId()
  {
    return SignInManager.UserManager.GetUserId(Context.BackofficeUser);
  }
}


public interface IAuthenticationService
{
  /// <summary>
  /// Get currently logged-in user
  /// </summary>
  Task<ZeroUser> GetUser();

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
  Task<LoginResult> Login(string email, string password, bool isPersistent);

  /// <summary>
  /// Logs out the current user
  /// </summary>
  Task Logout();

  /// <summary>
  /// Get the ID of the currently logged in user
  /// </summary>
  string GetUserId();
}