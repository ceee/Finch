using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class ApplicationContext : IApplicationContext
  {
    /// <inheritdoc />
    public IApplication App { get; protected set; }

    /// <inheritdoc />
    public string AppId { get; protected set; }

    protected IDocumentStore Raven { get; private set; }

    protected IZeroOptions Options { get; private set; }

    protected UserManager<User> UserManager { get; private set; }

    protected ILogger<ApplicationContext> Logger { get; private set; }

    static IList<IApplication> Apps { get; set; }



    public ApplicationContext(IDocumentStore raven, IZeroOptions options, UserManager<User> userManager, ILogger<ApplicationContext> logger)
    {
      Raven = raven;
      Options = options;
      UserManager = userManager;
      Logger = logger;
    }


    /// <inheritdoc />
    public async Task<IApplication> Resolve(HttpContext context)
    {
      if (context?.Request == null)
      {
        return null;
      }

      IApplication app;

      if (IsBackofficeRequest(context))
      {
        app = await ResolveFromUser(context.User);
      }
      else
      {
        app = await ResolveFromRequest(context);
      }

      if (app == null)
      {
        //Logger.LogWarning("Could not resolve application for host {host}", context.Request.Host);
        IList<IApplication> apps = await GetApplications();
        app = apps.FirstOrDefault();
      }

      App = app;
      AppId = app?.Id;

      return app;
    }


    /// <inheritdoc />
    public async Task<bool> TrySwitchForUser(User user, string appId)
    {
      if (user == null || appId.IsNullOrEmpty())
      {
        return false;
      }

      string[] allowedAppIds = GetAllowedAppIdsForUser(user);

      bool isMainId = appId.Equals(user.AppId, StringComparison.InvariantCultureIgnoreCase);
      bool isAllowedId = allowedAppIds.Contains(appId, StringComparer.InvariantCultureIgnoreCase);

      if (user.IsSuper || isMainId || isAllowedId)
      {
        user.CurrentAppId = appId;

        IdentityResult updateResult = await UserManager.UpdateAsync(user);
        return updateResult.Succeeded;
      }

      return false;
    }
       

    /// <inheritdoc />
    public async Task<IApplication> ResolveFromUser(ClaimsPrincipal user)
    {
      User userEntity = await UserManager.GetUserAsync(user);
      return await ResolveFromUser(userEntity);
    }


    /// <inheritdoc />
    public async Task<IApplication> ResolveFromUser(User user)
    {
      if (user == null)
      {
        return null;
      }

      string appId = null;
      string[] allowedAppIds = GetAllowedAppIdsForUser(user);

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

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.LoadAsync<Application>(appId);
      }
    }


    /// <inheritdoc />
    public async Task<IApplication> ResolveFromRequest(HttpContext context)
    {
      return await ResolveFromUri(context.Request.GetEncodedUrl());
    }


    /// <inheritdoc />
    public async Task<IApplication> ResolveFromUri(string uriString)
    {
      return ResolveFromUriInternal(new Uri(uriString, UriKind.Absolute), await GetApplications());
    }


    /// <inheritdoc />
    public async Task<IApplication> ResolveFromUri(Uri uri)
    {
      return ResolveFromUriInternal(uri, await GetApplications());
    }


    /// <inheritdoc />
    public async Task<IApplicationContext> ForId(string appId)
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      IApplication app = await session.LoadAsync<Application>(appId);

      return new ApplicationContext(Raven, Options, UserManager, Logger)
      {
        App = app,
        AppId = appId
      };
    }


    /// <inheritdoc />
    public bool IsBackofficeRequest(HttpContext context)
    {
      string path = Options.BackofficePath.EnsureStartsWith('/').TrimEnd('/');
      return context.Request.Path.ToString().StartsWith(path);
    }


    /// <summary>
    /// Get matching application from an URI
    /// </summary>
    IApplication ResolveFromUriInternal(Uri uri, IList<IApplication> apps)
    {
      string[] protocols = new string[3] { "https://", "http://", "//" };

      IApplication currentApp = null;

      foreach (IApplication app in apps)
      {
        if (app.Domains?.Length < 1)
        {
          continue;
        }

        foreach (Uri domain in app.Domains)
        {
          //string normalizedDomain = domain;

          //if (!protocols.Any(protocol => domain.StartsWith(protocol, StringComparison.OrdinalIgnoreCase)))
          //{
          //  normalizedDomain = "http://" + domain;
          //}

          //UriBuilder uriBuilder = new UriBuilder(normalizedDomain);

          //if (!uriBuilder.Uri.IsAbsoluteUri)
          //{
          //  continue;
          //}

          int compareResult = Uri.Compare(uri, domain, UriComponents.HostAndPort, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);

          if (compareResult == 0)
          {
            currentApp = app;
            break;
          }
        }
      }

      return currentApp;
    }


    /// <summary>
    /// Get all applications to choose from
    /// </summary>
    async Task<IList<IApplication>> GetApplications()
    {
      if (Apps != null)
      {
        return Apps;
      }
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        Apps = await session.Query<IApplication>().ToListAsync();
        return Apps;
      }
    }


    /// <summary>
    /// Get applications the user has access to
    /// </summary>
    string[] GetAllowedAppIdsForUser(User user)
    {
      IEnumerable<Permission> permissions = user.Claims
        .Where(claim => claim.Type == Constants.Auth.Claims.Permission && claim.Value.StartsWith(Permissions.Applications))
        .Select(x => Permission.FromClaim(x.ToClaim(), Permissions.Applications));

      string[] appIds = permissions.Where(x => x.IsTrue).Select(x => x.NormalizedKey).ToArray();

      return appIds;
    }
  }



  public interface IApplicationContext
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
    /// Resolves the current application from either the backoffice user (in case it is backoffice request)
    /// or the domain (in case it is frontend request).
    /// The resolved data is stored in the App + AppId properties.
    /// </summary>
    Task<IApplication> Resolve(HttpContext context);

    /// <summary>
    /// Try to switch the current application for a user
    /// </summary>
    Task<bool> TrySwitchForUser(User user, string appId);

    /// <summary>
    /// Resolves the current application from the request path
    /// </summary>
    Task<IApplication> ResolveFromRequest(HttpContext context);

    /// <summary>
    /// Get matching application from an URI string
    /// </summary>
    Task<IApplication> ResolveFromUri(string uriString);

    /// <summary>
    /// Get matching application from an URI
    /// </summary>
    Task<IApplication> ResolveFromUri(Uri uri);

    /// <summary>
    /// Resolves the current application from the logged-in backoffice user.
    /// This method won't return apps the user has no access to.
    /// </summary>
    Task<IApplication> ResolveFromUser(ClaimsPrincipal user);

    /// <summary>
    /// Resolves the current application from a user.
    /// This method won't return apps the user has no access to.
    /// </summary>
    Task<IApplication> ResolveFromUser(User user);

    /// <summary>
    /// Creates a new application context for the specified application.
    /// </summary>
    Task<IApplicationContext> ForId(string appId);

    /// <summary>
    /// Whether the current request is a backoffice request
    /// </summary>
    bool IsBackofficeRequest(HttpContext context);
  }
}
