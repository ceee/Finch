using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Core.Api
{
  public class UserRolesApi : IUserRolesApi
  {
    protected IDocumentStore Raven { get; private set; }

    protected IHttpContextAccessor HttpContextAccessor { get; set; }

    protected UserManager<User> UserManager { get; private set; }

    private ClaimsPrincipal Principal => HttpContextAccessor.HttpContext?.User;


    public UserRolesApi(IDocumentStore raven, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
    {
      Raven = raven;
      HttpContextAccessor = httpContextAccessor;
      UserManager = userManager;
    }


    /// <inheritdoc />
    public async Task<User> GetUser()
    {
      User user = await UserManager.GetUserAsync(HttpContextAccessor.HttpContext.User);
      return user;
    }


    /// <inheritdoc />
    public async Task<User> GetUserById(string id)
    {
      User user = await UserManager.FindByIdAsync(id);
      return user;
    }


    /// <inheritdoc />
    public async Task<User> GetUserByEmail(string email)
    {
      User user = await UserManager.FindByEmailAsync(email);
      return user;
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
        .Select(claim => new Permission(claim, prefix))
        .ToList();
    }
  }


  public interface IUserRolesApi
  {
    /// <summary>
    /// Get currently logged-in user
    /// </summary>
    Task<User> GetUser();

    /// <summary>
    /// Find user by id
    /// </summary>
    Task<User> GetUserById(string id);

    /// <summary>
    /// Find user by email
    /// </summary>
    Task<User> GetUserByEmail(string email);

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
  }
}
