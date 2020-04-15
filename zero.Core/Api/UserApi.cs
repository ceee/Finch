using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Api
{
  public class UserApi : IUserApi
  {
    protected IDocumentStore Raven { get; private set; }

    protected IHttpContextAccessor HttpContextAccessor { get; set; }

    protected UserManager<User> UserManager { get; private set; }


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
  }
}
