using Microsoft.AspNetCore.Http;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Api
{
  public class AuthenticationApi : IAuthenticationApi
  {
    protected IDocumentStore Raven { get; private set; }

    protected IHttpContextAccessor HttpContextAccessor { get; set; }


    public AuthenticationApi(IDocumentStore raven, IHttpContextAccessor httpContextAccessor)
    {
      Raven = raven;
      HttpContextAccessor = httpContextAccessor;
    }


    /// <inheritdoc />
    public bool IsLoggedIn()
    {
      return HttpContextAccessor.HttpContext.User != null;
    }


    /// <inheritdoc />
    public string GetUserId()
    {
      return null;
      //ResolveIdFromClaimsPrincipal(HttpContextAccessor.HttpContext.User);
    }


    /// <inheritdoc />
    public async Task<User> GetUser()
    {
      return null;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        
      }
    }
  }


  public interface IAuthenticationApi
  {
    /// <summary>
    /// Whether a user is currently logged-in
    /// </summary>
    bool IsLoggedIn();

    /// <summary>
    /// Get the ID of the currently logged in user
    /// </summary>
    string GetUserId();

    /// <summary>
    /// Get the currently logged-in user (or null if not logged in)
    /// </summary>
    Task<User> GetUser();
  }
}
