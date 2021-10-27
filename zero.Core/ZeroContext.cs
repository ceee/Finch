using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;
using System;
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
using zero.Core.Utils;

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
    public bool IsLoggedIntoBackoffice { get; protected set; }

    /// <inheritdoc />
    public IZeroOptions Options { get; protected set; }

    /// <inheritdoc />
    public Route Route => ResolvedRoute?.Route;

    /// <inheritdoc />
    public IResolvedRoute ResolvedRoute => HttpContextAccessor?.HttpContext?.Features.Get<IResolvedRoute>();

    /// <inheritdoc />
    public IZeroStore Store { get; private set; }


    protected IApplicationResolver AppResolver { get; private set; }

    protected ICultureResolver CultureResolver { get; private set; }

    protected ILogger<ZeroContext> Logger { get; private set; }

    protected IHandlerHolder Handler { get; private set; }

    protected IHttpContextAccessor HttpContextAccessor { get; private set; }

    protected IPrimitiveTypeCollection ValueCollection { get; private set; }


    bool _resolved = false;


    public ZeroContext(IZeroOptions options, IHttpContextAccessor httpContextAccessor, IApplicationResolver appResolver, ICultureResolver cultureResolver, ILogger<ZeroContext> logger, IZeroStore store, IHandlerHolder handler)
    {
      Options = options;
      AppResolver = appResolver;
      CultureResolver = cultureResolver;
      Logger = logger;
      Store = store;
      Handler = handler;
      ValueCollection = new PrimitiveTypeCollection();
      HttpContextAccessor = httpContextAccessor;
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

      // set default database for document store
      Store.ResolvedDatabase = Application.Database;

      // set current culture
      await CultureResolver.Resolve(this);
    }


    /// <inheritdoc />
    public void Override(Application app)
    {
      Application = app;
      AppId = app?.Id;
      Store.ResolvedDatabase = app?.Database;
    }


    /// <inheritdoc />
    public ZeroContextScope CreateScope(Application app)
    {
      return new(this, app);
    }


    /// <inheritdoc />
    public T Get<T>() => ValueCollection.Get<T>();


    /// <inheritdoc />
    public void Set<T>(T value) => ValueCollection.Set(value);


    /// <inheritdoc />
    public void Remove<T>() => ValueCollection.Remove<T>();
  }



  public class ZeroContextScope : IDisposable
  {
    public IZeroContext Context { get; }

    Application _originalApp = null;

    public ZeroContextScope(IZeroContext context, Application app)
    {
      Context = context;
      _originalApp = app;
      Context.Override(app);
    }

    /// <inheritdoc />
    public void Dispose()
    {
      Context.Override(_originalApp);
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
    /// Overrides the resolved application for this context instance
    /// </summary>
    void Override(Application app);

    /// <summary>
    /// SCOPE
    /// </summary>
    ZeroContextScope CreateScope(Application app);

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
  }
}
