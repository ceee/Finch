using Microsoft.AspNetCore.Identity;

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
  public async Task<Result> Login(string email, string password, bool isPersistent)
  {
    Result result = new();

    ZeroUser user = await SignInManager.UserManager.FindByNameAsync(email);

    if (user == null)
    {
      result.AddError("@login.errors.wrongcredentials"); // TODO we don't need translations here, but return an enum, so the app itself can translate the error
      return result;
    }
    // TODO probably move this logic into a custom SignInManager which overrides CanSignInAsync()
    // see https://stackoverflow.com/a/35484758/670860
    else if (!user.IsActive)
    {
      result.AddError("@login.errors.disabled");
      return result;
    }

    SignInResult signInResult = await SignInManager.PasswordSignInAsync(user, password, isPersistent, true);

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

    return Result.Success();
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
  Task<Result> Login(string email, string password, bool isPersistent);

  /// <summary>
  /// Logs out the current user
  /// </summary>
  Task Logout();

  /// <summary>
  /// Get the ID of the currently logged in user
  /// </summary>
  string GetUserId();
}