using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;

namespace zero.Core.Api
{
  public class AuthenticationApi : IAuthenticationApi
  {
    protected IZeroContext Context { get; set; }

    protected SignInManager<BackofficeUser> SignInManager { get; private set; }

    protected IZeroDocumentSession Session { get; set; }


    public AuthenticationApi(IZeroContext context, SignInManager<BackofficeUser> signInManager, IZeroDocumentSession session)
    {
      Context = context;
      SignInManager = signInManager;
      Session = session.Core;
    }


    /// <inheritdoc />
    public bool IsLoggedIn()
    {
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
    public async Task<BackofficeUser> GetUser()
    {
      return await SignInManager.UserManager.GetUserAsync(Context.BackofficeUser);
    }


    /// <inheritdoc />
    public IList<Permission> GetPermissions(string prefix = null)
    {
      return Context.BackofficeUser.Claims
        .Where(claim => claim.Type == Constants.Auth.Claims.Permission && (prefix == null || claim.Value.StartsWith(prefix)))
        .Select(claim => Permission.FromClaim(claim, prefix))
        .ToList();
    }


    /// <inheritdoc />
    public Permission GetPermission(string key = null)
    {
      Claim claim = Context.BackofficeUser.Claims.FirstOrDefault(claim => claim.Type == Constants.Auth.Claims.Permission && claim.Value.StartsWith(key + ":"));
      return Permission.FromClaim(claim);
    }


    /// <inheritdoc />
    public async Task<EntityResult> Login(string email, string password, bool isPersistent)
    {
      EntityResult result = new EntityResult();

      BackofficeUser user = await SignInManager.UserManager.FindByNameAsync(email);

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
      return SignInManager.UserManager.GetUserId(Context.BackofficeUser);
    }


    /// <inheritdoc />
    public async Task<bool> TrySwitchApp(string appId)
    {
      BackofficeUser user = await GetUser();

      if (user == null || appId.IsNullOrEmpty())
      {
        return false;
      }

      string[] allowedAppIds = user.GetAllowedAppIds();

      bool isMainId = appId.Equals(user.AppId, StringComparison.InvariantCultureIgnoreCase);
      bool isAllowedId = allowedAppIds.Contains(appId, StringComparer.InvariantCultureIgnoreCase);

      if (user.IsSuper || isMainId || isAllowedId)
      {
        user.CurrentAppId = appId;

        //byte[] bytes = new byte[20];
        //RandomNumberGenerator.Fill(bytes);
        //user.SecurityStamp = Base32.ToBase32(bytes); // TODO update security stamp but Base32 is .net core internal

        await Session.StoreAsync(user);
        await Session.SaveChangesAsync();

        return true;
      }

      return false;
    }
  }


  public interface IAuthenticationApi
  {
    /// <summary>
    /// Get currently logged-in user
    /// </summary>
    Task<BackofficeUser> GetUser();

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
    Permission GetPermission(string key = null);

    /// <summary>
    /// Tries to switch the currently loaded backoffice application for the current user
    /// </summary>
    Task<bool> TrySwitchApp(string appId);
  }
}
