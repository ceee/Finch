using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Security.Claims;

namespace zero.Applications;

public class ApplicationResolver : IApplicationResolver
{
  protected IZeroStore Store { get; private set; }

  protected IZeroOptions Options { get; private set; }

  protected ILogger<ApplicationResolver> Logger { get; private set; }

  protected IHandlerHolder Handler { get; private set; }

  IList<Application> _apps;

  /// <summary>
  /// The system application is bound to the context in case no application was resolved
  /// </summary>
  SystemApplication _systemApplication;



  public ApplicationResolver(IZeroStore store, IZeroOptions options, ILogger<ApplicationResolver> logger, IHandlerHolder handler = null)
  {
    Store = store;
    Options = options;
    Logger = logger;
    Handler = handler;

    _systemApplication = new()
    {
      Id = "system",
      Name = "zero system",
      Database = Options.For<RavenOptions>().Database,
      Alias = "system"
    };
  }


  /// <inheritdoc />
  public async Task<Application> Resolve(HttpContext context, ClaimsPrincipal user)
  {
    if (context?.Request == null)
    {
      Logger.LogWarning("Could not resolve application as HTTP request is null");
      return _systemApplication;
    }

    Application app;

    if (context.IsBackofficeRequest(Options.ZeroPath))
    {
      ZeroUser userEntity = await GetBackofficeUser(user);
      app = await ResolveFromBackofficeHandlers(context, userEntity) ?? _systemApplication;
    }
    else
    {
      app = await ResolveFromHandlers(context) ?? await ResolveFromRequest(context);
    }

    if (app == null)
    {
      Logger.LogWarning("Could not resolve application for host {host}", context.Request.Host);
      return _systemApplication;
    }

    return app;
  }


  /// <inheritdoc />
  public async Task<Application> ResolveFromHandlers(HttpContext context)
  {
    IList<Application> apps = await GetApplications();
    IEnumerable<IApplicationResolverHandler> handlers = Handler.GetAll<IApplicationResolverHandler>();

    foreach (IApplicationResolverHandler handler in handlers)
    {
      if (handler.TryResolve(context, apps, out Application resolved))
      {
        return resolved;
      }
    }

    return null;
  }


  /// <inheritdoc />
  public async Task<Application> ResolveFromBackofficeHandlers(HttpContext context, ZeroUser user)
  {
    IList<Application> apps = await GetApplications();
    IEnumerable<IBackofficeApplicationResolverHandler> handlers = Handler.GetAll<IBackofficeApplicationResolverHandler>();

    foreach (IBackofficeApplicationResolverHandler handler in handlers)
    {
      if (handler.TryResolve(context, apps, user, out Application resolved))
      {
        return resolved;
      }
    }

    return null;
  }


  /// <inheritdoc />
  public async Task<Application> ResolveFromUser(ClaimsPrincipal user)
  {
    ZeroUser userEntity = await GetBackofficeUser(user);
    return await ResolveFromUser(userEntity);
  }


  /// <inheritdoc />
  public async Task<Application> ResolveFromUser(ZeroUser user)
  {
    if (user == null)
    {
      return null;
    }

    string appId;
    string[] allowedAppIds = user.GetAllowedAppIds();

    if (!user.CurrentAppId.IsNullOrEmpty())
    {
      if (user.IsSuper || allowedAppIds.Contains(user.CurrentAppId, StringComparer.InvariantCultureIgnoreCase))
      {
        appId = user.CurrentAppId;
      }
      else
      {
        appId = user.AppId;
      }
    }
    else
    {
      appId = user.AppId;
    }

    if (appId.IsNullOrEmpty())
    {
      throw new Exception($"User entity ${user.Id} needs a valid AppId");
    }

    IAsyncDocumentSession session = Store.Session(global: true);
    return await session.LoadAsync<Application>(appId);
  }


  /// <inheritdoc />
  public Task<Application> ResolveFromRequest(HttpContext context) => ResolveFromUri(context.Request.GetEncodedUrl());


  /// <inheritdoc />
  public async Task<Application> ResolveFromUri(string uriString) => ResolveFromUriInternal(new Uri(uriString, UriKind.Absolute), await GetApplications());


  /// <inheritdoc />
  public async Task<Application> ResolveFromUri(Uri uri) => ResolveFromUriInternal(uri, await GetApplications());


  /// <summary>
  /// Get matching application from an URI
  /// </summary>
  Application ResolveFromUriInternal(Uri uri, IList<Application> apps)
  {
    foreach (Application app in apps)
    {
      if (app.Domains?.Length < 1)
      {
        Logger.LogWarning("No domains specified for app {app}", app.Id);
        continue;
      }

      foreach (Uri domain in app.Domains)
      {
        int compareResult = Uri.Compare(uri, domain, UriComponents.HostAndPort, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);
        if (compareResult == 0)
        {
          return app;
        }
      }
    }

    return null;
  }


  /// <summary>
  /// Get all applications to choose from
  /// </summary>
  async Task<IList<Application>> GetApplications()
  {
    if (_apps != null)
    {
      return _apps;
    }

    IAsyncDocumentSession session = Store.Session(global: true);
    _apps = await session.Query<Application>().ToListAsync();
    return _apps;
  }


  /// <summary>
  /// Get backoffice user from claims principal
  /// </summary>
  async Task<ZeroUser> GetBackofficeUser(ClaimsPrincipal user)
  {
    string userId = user.FindFirstValue(Constants.Auth.Claims.UserId);

    IAsyncDocumentSession session = Store.Session(global: true);
    return await session.LoadAsync<ZeroUser>(userId);
  }
}


public interface IApplicationResolver
{
  /// <summary>
  /// Resolves the current application from either the backoffice user (in case it is backoffice request)
  /// or the domain (in case it is frontend request).
  /// </summary>
  Task<Application> Resolve(HttpContext context, ClaimsPrincipal user);

  /// <summary>
  /// Resolves the current application from the request path
  /// </summary>
  Task<Application> ResolveFromRequest(HttpContext context);

  /// <summary>
  /// Get matching application from an URI string
  /// </summary>
  Task<Application> ResolveFromUri(string uriString);

  /// <summary>
  /// Get matching application from an URI
  /// </summary>
  Task<Application> ResolveFromUri(Uri uri);

  /// <summary>
  /// Resolves the current application from the logged-in backoffice user.
  /// This method won't return apps the user has no access to.
  /// </summary>
  Task<Application> ResolveFromUser(ClaimsPrincipal user);

  /// <summary>
  /// Resolves the current application from a user.
  /// This method won't return apps the user has no access to.
  /// </summary>
  Task<Application> ResolveFromUser(ZeroUser user);
}