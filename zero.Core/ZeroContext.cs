using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;
using zero.Core.Routing;

namespace zero.Core
{
  public class ZeroContext : IZeroContext
  {
    /// <inheritdoc />
    public IApplication Application { get; protected set; }

    /// <inheritdoc />
    public string AppId { get; protected set; }

    /// <inheritdoc />
    public ClaimsPrincipal BackofficeUser { get; protected set; }

    /// <inheritdoc />
    public bool IsBackofficeRequest { get; protected set; }

    /// <inheritdoc />
    public IZeroOptions Options { get; protected set; }

    /// <inheritdoc />
    public IRoute Route { get; private set; }

    /// <inheritdoc />
    public IResolvedRoute ResolvedRoute { get; private set; }


    protected IApplicationResolver AppResolver { get; private set; }

    protected ILogger<ZeroContext> Logger { get; private set; }

    protected IZeroStore Store { get; private set; }


    private bool _resolved = false;


    public ZeroContext(IZeroOptions options, IApplicationResolver appResolver, ILogger<ZeroContext> logger, IZeroStore store)
    {
      Options = options;
      AppResolver = appResolver;
      Logger = logger;
      Store = store;
    }


    /// <inheritdoc />
    public async virtual Task Resolve(HttpContext context, IEnumerable<IApplication> applications)
    {
      if (_resolved)
      {
        return;
      }

      if (context?.Request is null)
      {
        Store.ResolvedDatabase = null;
        return;
      }

      _resolved = true;

      // check if the current request is a backoffice request
      IsBackofficeRequest = context.IsBackofficeRequest(Options.BackofficePath);

      // get the currently logged-in backoffice user
      BackofficeUser = new ClaimsPrincipal();
      AuthenticateResult authResult = await context.AuthenticateAsync(Constants.Auth.BackofficeScheme);
      if (authResult?.Principal is not null)
      {
        BackofficeUser = authResult.Principal;
      }

      // resolve current application
      Application = await AppResolver.Resolve(applications, context, BackofficeUser);
      AppId = Application.Id;

      // set default database for document store
      Store.ResolvedDatabase = Application.Database;
    }


    /// <inheritdoc />
    public virtual void SetRoute(IResolvedRoute route)
    {
      ResolvedRoute = route;
      Route = ResolvedRoute.Route;
    }
  }


  public interface IZeroContext
  {
    /// <summary>
    /// Currently loaded application
    /// </summary>
    IApplication Application { get; }

    /// <summary>
    /// Current loaded application Id
    /// </summary>
    string AppId { get; }

    /// <summary>
    /// Resolved backoffice user principal
    /// </summary>
    ClaimsPrincipal BackofficeUser { get; }

    /// <summary>
    /// Whether the current request is a backoffice request or not
    /// </summary>
    bool IsBackofficeRequest { get; }

    /// <summary>
    /// Global zero options
    /// </summary>
    IZeroOptions Options { get; }

    /// <summary>
    /// Matching (frontend) path route
    /// </summary>
    IRoute Route { get; }

    /// <summary>
    /// Matching (frontend) resolved route
    /// </summary>
    IResolvedRoute ResolvedRoute { get; }

    /// <summary>
    /// Resolves the current application (for backoffice + frontend requests) and
    /// the currently active backoffice user, as users are not signed in with the default scheme and do therefore not populate HttpContext.User
    /// </summary>
    Task Resolve(HttpContext context, IEnumerable<IApplication> applications);

    /// <summary>
    /// Set resolved route for frontend requests
    /// </summary>
    void SetRoute(IResolvedRoute route);
  }
}
