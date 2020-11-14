using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Core.Api
{
  public class AuthorizationApi : IAuthorizationApi
  {
    protected IDocumentStore Raven { get; private set; }

    protected IHttpContextAccessor HttpContextAccessor { get; set; }

    protected SignInManager<BackofficeUser> SignInManager { get; private set; }

    protected ClaimsPrincipal Principal => HttpContextAccessor.HttpContext?.User;


    public AuthorizationApi(IDocumentStore raven, IHttpContextAccessor httpContextAccessor, SignInManager<BackofficeUser> signInManager)
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


    public EntityPermission GetPermissionForEntity<T>(T model, string permissionKey) where T : IZeroEntity
    {
      EntityPermission result = new EntityPermission();

      if (!IsLoggedIn())
      {
        return result;
      }

      Type type = typeof(T);
      bool isSuperUser = Principal.HasClaim(Constants.Auth.Claims.IsSuper, PermissionsValue.True);

      //result.IsAppAware = AppAwareType.IsAssignableFrom(type); // TODO appx
      //result.IsShareable = result.IsAppAware && AppAwareShareableType.IsAssignableFrom(type);
      
      if (isSuperUser)
      {
        result.CanCreate = true;
        result.CanCreateShared = result.CanCreate && result.IsShareable;
        result.CanEdit = true;
        result.CanRead = true;
        result.CanDelete = true;
        return result;
      }

      Permission permission = GetPermission(permissionKey);

      if (permission != null)
      {

      }

      return result;
    }
  }


  public interface IAuthorizationApi
  {
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
    /// Get all permissions for the current user with an optional prefix
    /// </summary>
    IList<Permission> GetPermissions(string prefix = null);

    /// <summary>
    /// Get a single permissions by key
    /// </summary>
    public Permission GetPermission(string key = null);
  }
}
