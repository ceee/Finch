using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Cultures;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Handlers;
using zero.Core.Options;
using zero.Core.Routing;

namespace zero.Core
{
  public class ZeroContext : IZeroContext
  {
    /// <inheritdoc />
    public Application Application { get; protected set; }

    /// <inheritdoc />
    public string AppId { get; protected set; }

    /// <inheritdoc />
    public ClaimsPrincipal BackofficeUser { get; protected set; }

    /// <inheritdoc />
    public bool IsBackofficeRequest { get; protected set; }

    /// <inheritdoc />
    public IZeroOptions Options { get; protected set; }

    /// <inheritdoc />
    public Route Route { get; private set; }

    /// <inheritdoc />
    public IResolvedRoute ResolvedRoute { get; private set; }

    /// <inheritdoc />
    public IZeroStore Store { get; private set; }


    protected IApplicationResolver AppResolver { get; private set; }

    protected ICultureResolver CultureResolver { get; private set; }

    protected ILogger<ZeroContext> Logger { get; private set; }

    protected IHandlerHolder Handler { get; private set; }


    bool _resolved = false;

    ConcurrentDictionary<string, object> _properties = new();

    ConcurrentDictionary<string, IAsyncDocumentSession> _sessions = new();


    public ZeroContext(IZeroOptions options, IApplicationResolver appResolver, ICultureResolver cultureResolver, ILogger<ZeroContext> logger, IZeroStore store, IHandlerHolder handler)
    {
      Options = options;
      AppResolver = appResolver;
      CultureResolver = cultureResolver;
      Logger = logger;
      Store = store;
      Handler = handler;
    }


    /// <inheritdoc />
    public async virtual Task Resolve(HttpContext context)
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

      if (!Options.SetupCompleted)
      {
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
      Application = await AppResolver.Resolve(context, BackofficeUser);
      AppId = Application.Id;

      // set default database for document store
      Store.ResolvedDatabase = Application.Database;

      // set current culture
      await CultureResolver.Resolve(this);      

      // resolve request route
      if (IsBackofficeRequest is false && context.Request.RouteValues.TryGetValue("zero.route", out object route))
      {
        ResolvedRoute = (IResolvedRoute)route;
        Route = ResolvedRoute.Route;
      }

      IContextResolverHandler handler = Handler.Get<IContextResolverHandler>();
      if (handler != null)
      {
        await handler.AfterResolve(this);
      }
    }


    internal void SetRoute(IResolvedRoute route)
    {
      ResolvedRoute = route;
      Route = ResolvedRoute.Route;
    }


    /// <inheritdoc />
    public T GetProperty<T>(string key)
    {
      if (_properties.TryGetValue(key, out object value) && value is T)
      {
        return (T)value;
      }
      return default;
    }


    /// <inheritdoc />
    public void SetProperty(string key, object value)
    {
      _properties[key] = value;
    }


    /// <inheritdoc />
    public void RemoveProperty(string key)
    {
      _properties.TryRemove(key, out _);
    }


    /// <inheritdoc />
    public IAsyncDocumentSession GetCurrentScopeAsyncSession(string database = null)
    {
      database ??= Store.ResolvedDatabase;
      return _sessions.GetOrAdd(database, _ => Store.OpenAsyncSession(database));
    }
  }


  public interface IZeroContext
  {
    /// <summary>
    /// Currently loaded application
    /// </summary>
    Application Application { get; }

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
    /// Document store
    /// </summary>
    IZeroStore Store { get; }

    /// <summary>
    /// Matching (frontend) path route
    /// </summary>
    Route Route { get; }

    /// <summary>
    /// Matching (frontend) resolved route
    /// </summary>
    IResolvedRoute ResolvedRoute { get; }

    /// <summary>
    /// Resolves the current application (for backoffice + frontend requests) and
    /// the currently active backoffice user, as users are not signed in with the default scheme and do therefore not populate HttpContext.User
    /// </summary>
    Task Resolve(HttpContext context);

    /// <summary>
    /// When using one session per request, we can retrieve the current session for this request
    /// </summary>
    IAsyncDocumentSession GetCurrentScopeAsyncSession(string database = null);

    /// <summary>
    /// Get a custom property from this scoped context
    /// </summary>
    T GetProperty<T>(string key);

    /// <summary>
    /// Add a custom property to this scoped context
    /// </summary>
    void SetProperty(string key, object value);

    /// <summary>
    /// Remove a custom property from this scoped context
    /// </summary>
    void RemoveProperty(string key);
  }
}
