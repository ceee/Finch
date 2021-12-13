using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace zero.Context;

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
  public bool IsLoggedIntoBackoffice { get; protected set; }

  /// <inheritdoc />
  public IZeroOptions Options { get; protected set; }

  /// <inheritdoc />
  public Route Route => ResolvedRoute?.Route;

  /// <inheritdoc />
  public IRouteModel ResolvedRoute => HttpContextAccessor?.HttpContext?.Features.Get<IRouteModel>();

  /// <inheritdoc />
  public IZeroStore Store { get; private set; }

  /// <inheritdoc />
  public IServiceProvider Services { get; private set; }

  /// <inheritdoc />
  public ZeroContextScope Scope { get; private set; }


  protected IApplicationResolver AppResolver { get; private set; }

  protected ICultureResolver CultureResolver { get; private set; }

  protected ILogger<ZeroContext> Logger { get; private set; }

  protected IHandlerHolder Handler { get; private set; }

  protected IHttpContextAccessor HttpContextAccessor { get; private set; }

  protected IPrimitiveTypeCollection ValueCollection { get; private set; }


  bool _resolved = false;


  public ZeroContext(IZeroOptions options, IHttpContextAccessor httpContextAccessor, IApplicationResolver appResolver, ICultureResolver cultureResolver, 
    ILogger<ZeroContext> logger, IZeroStore store, IHandlerHolder handler, IServiceProvider services)
  {
    Options = options;
    AppResolver = appResolver;
    CultureResolver = cultureResolver;
    Logger = logger;
    Store = store;
    Handler = handler;
    ValueCollection = new PrimitiveTypeCollection();
    HttpContextAccessor = httpContextAccessor;
    Services = services;
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

    //if (!Options.SetupCompleted)
    //{
    //  return;
    //} // TODO setup

    _resolved = true;

    // check if the current request is a backoffice request
    IsBackofficeRequest = context.IsBackofficeRequest(Options.ZeroPath);

    // get the currently logged-in backoffice user
    BackofficeUser = new ClaimsPrincipal();
    IsLoggedIntoBackoffice = false;
    AuthenticateResult authResult = await context.AuthenticateAsync(Constants.Auth.BackofficeScheme);
    if (authResult?.Principal is not null)
    {
      BackofficeUser = authResult.Principal;
      IsLoggedIntoBackoffice = true;
    }

    // resolve current application
    Application = await AppResolver.Resolve(context, BackofficeUser);
    AppId = Application.Id;

    Logger.LogInformation("Resolved {appId} ({uri})", AppId, context.Request.Host.ToString() + context.Request.Path.Value.EnsureStartsWith('/'));

    // set default database for document store
    Store.ResolvedDatabase = Application.Database;

    // set current culture
    await CultureResolver.Resolve(this);

    // set context scope 
    Scope = new(Store, Store.ResolvedDatabase, Application);
  }


  /// <inheritdoc />
  public T Get<T>() => ValueCollection.Get<T>();


  /// <inheritdoc />
  public void Set<T>(T value) => ValueCollection.Set(value);


  /// <inheritdoc />
  public void Remove<T>() => ValueCollection.Remove<T>();


  /// <inheritdoc />
  public ZeroContextScope CreateScope(Application app)
  {
    ApplyScope(app.Database, app);
    return new ZeroContextScope(Store, app.Database, app, Scope, scope =>
    {
      Scope = scope.Previous;
      ApplyScope(Scope.Database, Scope.Application);
    });
  }


  /// <inheritdoc />
  public ZeroContextScope CreateScope(string database)
  {
    ApplyScope(database);
    return new ZeroContextScope(Store, database, null, Scope, scope =>
    {
      Scope = scope.Previous;
      ApplyScope(Scope.Database, Scope.Application);
    });
  }


  /// <summary>
  /// Apply a database scope
  /// </summary>
  void ApplyScope(string database, Application app = null)
  {
    Application = app;
    AppId = app?.Id;
    Store.ResolvedDatabase = database;
  }
}



public class ZeroContextScope : IDisposable
{
  public ZeroContextScope(IZeroStore store, string database, Application application, ZeroContextScope previous = null, Action<ZeroContextScope> onDispose = null)
  {
    Store = store;
    Database = database;
    Application = application;
    Previous = previous;
    _onDispose = onDispose;
  }

  public IZeroStore Store { get; set; }

  public Application Application { get; private set; }

  public string Database { get; private set; }

  public ZeroContextScope Previous { get; private set; }

  Action<ZeroContextScope> _onDispose = null;

  public void Dispose()
  {
    _onDispose?.Invoke(this);
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
  /// Whether the user is logged into the backoffice
  /// </summary>
  bool IsLoggedIntoBackoffice { get; }

  /// <summary>
  /// Global zero options
  /// </summary>
  IZeroOptions Options { get; }

  /// <summary>
  /// Document store
  /// </summary>
  IZeroStore Store { get; }

  /// <summary>
  /// Service container
  /// </summary>
  IServiceProvider Services { get; }

  /// <summary>
  /// Current context scope
  /// </summary>
  ZeroContextScope Scope { get; }

  /// <summary>
  /// Matching (frontend) path route
  /// </summary>
  Route Route { get; }

  /// <summary>
  /// Matching (frontend) resolved route
  /// </summary>
  IRouteModel ResolvedRoute { get; }

  /// <summary>
  /// Resolves the current application (for backoffice + frontend requests) and
  /// the currently active backoffice user, as users are not signed in with the default scheme and do therefore not populate HttpContext.User
  /// </summary>
  Task Resolve(HttpContext context);

  /// <summary>
  /// Get a custom property from this scoped context
  /// </summary>
  T Get<T>();

  /// <summary>
  /// Add a custom property to this scoped context
  /// </summary>
  void Set<T>(T value);

  /// <summary>
  /// Remove a custom property from this scoped context
  /// </summary>
  void Remove<T>();

  /// <summary>
  /// Scope the current context to a specific application database
  /// </summary>
  ZeroContextScope CreateScope(Application app);

  /// <summary>
  /// Scope the current context to a specific database
  /// </summary>
  ZeroContextScope CreateScope(string database);
}