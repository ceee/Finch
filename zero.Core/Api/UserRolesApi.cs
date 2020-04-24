using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
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

    protected RoleManager<UserRole> RoleManager { get; private set; }

    private ClaimsPrincipal Principal => HttpContextAccessor.HttpContext?.User;


    public UserRolesApi(IDocumentStore raven, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, RoleManager<UserRole> roleManager)
    {
      Raven = raven;
      HttpContextAccessor = httpContextAccessor;
      UserManager = userManager;
      RoleManager = roleManager;
    }


    /// <inheritdoc />
    public async Task<IList<UserRole>> GetAll()
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<UserRole>().OrderBy(x => x.Sort).ThenBy(x => x.Name).ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<UserRole> GetById(string id)
    {
      return await RoleManager.FindByIdAsync(id);
    }


    /// <inheritdoc />
    //public IList<Permission> GetPermissions(string prefix = null)
    //{
    //  return Principal.Claims
    //    .Where(claim => claim.Type == Constants.Auth.Claims.Permission && (prefix == null || claim.Value.StartsWith(prefix)))
    //    .Select(claim => new Permission(claim, prefix))
    //    .ToList();
    //}
  }


  public interface IUserRolesApi
  {
    /// <summary>
    /// Get all user roles
    /// </summary>
    Task<IList<UserRole>> GetAll();

    /// <summary>
    /// Get role by id
    /// </summary>
    Task<UserRole> GetById(string id);
  }
}
