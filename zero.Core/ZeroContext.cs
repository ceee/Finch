using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Options;

namespace zero.Core
{
  public class ZeroContext : IZeroContext
  {
    /// <inheritdoc />
    public IApplication App { get; protected set; }

    /// <inheritdoc />
    public string AppId { get; protected set; }

    /// <inheritdoc />
    public ClaimsPrincipal User { get; protected set; }

    /// <inheritdoc />
    public ClaimsIdentity Identity { get; protected set; }


    protected IZeroOptions Options { get; private set; }

    protected IApplicationContext AppContext { get; private set; }

    protected ILogger<ZeroContext> Logger { get; private set; }


    public ZeroContext(IZeroOptions options, IApplicationContext appContext, ILogger<ZeroContext> logger)
    {
      Options = options;
      AppContext = appContext;
      Logger = logger;
    }


    /// <inheritdoc />
    public async Task Resolve(HttpContext context)
    {
      if (context?.Request == null)
      {
        return;
      }

      AuthenticateResult authResult = await context.AuthenticateAsync(Constants.Auth.BackofficeScheme);
      if (authResult?.Principal != null)
      {
        User = authResult.Principal;
        if (BackofficeUserIdentity.TryGet(authResult.Principal, out BackofficeUserIdentity identity))
        {
          Identity = identity;
        }
      }
      else
      {
        User = new ClaimsPrincipal();
      }

      App = await AppContext.Resolve(context, User);
      AppId = App.Id;
    }
  }


  public interface IZeroContext
  {
    /// <summary>
    /// Currently loaded application
    /// </summary>
    IApplication App { get; }

    /// <summary>
    /// Current loaded application Id
    /// </summary>
    string AppId { get; }

    /// <summary>
    /// Resolved backoffice user principal
    /// </summary>
    ClaimsPrincipal User { get; }

    /// <summary>
    /// Resolved backoffice user identity
    /// </summary>
    ClaimsIdentity Identity { get; }

    /// <summary>
    /// Resolves the current application (for backoffice + frontend requests) and
    /// the currently active backoffice user, as users are not signed in with the default scheme and do therefore not populate HttpContext.User
    /// </summary>
    Task Resolve(HttpContext context);
  }
}
