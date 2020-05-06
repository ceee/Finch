using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;

namespace zero.Core.Api
{
  public class UserApi : IUserApi
  {
    protected IDocumentStore Raven { get; private set; }

    protected IHttpContextAccessor HttpContextAccessor { get; set; }

    protected UserManager<User> UserManager { get; private set; }

    private ClaimsPrincipal Principal => HttpContextAccessor.HttpContext?.User;


    public UserApi(IDocumentStore raven, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
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
    public async Task<IList<User>> GetAll(string appId = null)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<User>()
          .ForApp(appId)
          .OrderByDescending(x => x.CreatedDate)
          .ToListAsync();
      }
    }

    /// <inheritdoc />
    public async Task<ListResult<User>> GetByQuery(ListQuery<User> query, string appId = null)
    {
      query.SearchSelector = user => user.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<User>()
          .ForApp(appId)
          .ToQueriedListAsync(query);
      }
    }
  }


  public interface IUserApi
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

    /// <summary>
    /// Get a single permissions by key
    /// </summary>
    public Permission GetPermission(string key = null);

    /// <summary>
    /// Get all users for the selected application
    /// </summary>
    Task<IList<User>> GetAll(string appId = null);

    /// <summary>
    /// Get all available users (with query)
    /// </summary>
    Task<ListResult<User>> GetByQuery(ListQuery<User> query, string appId = null);
  }
}
