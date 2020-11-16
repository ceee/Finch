using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Handlers;
using zero.Core.Identity;
using zero.Core.Options;

namespace zero.Core
{
  public class ApplicationResolver : IApplicationResolver
  {
    protected IZeroStore Store { get; private set; }

    protected IZeroOptions Options { get; private set; }

    protected ILogger<ApplicationResolver> Logger { get; private set; }

    protected IHandlerHolder Handler { get; private set; }

    private IList<IApplication> Apps { get; set; }



    public ApplicationResolver(IZeroStore store, IZeroOptions options, ILogger<ApplicationResolver> logger, IHandlerHolder handler = null)
    {
      Store = store;
      Options = options;
      Logger = logger;
      Handler = handler;
    }


    /// <inheritdoc />
    public async Task<IApplication> Resolve(HttpContext context, ClaimsPrincipal user)
    {
      if (context?.Request == null)
      {
        Logger.LogWarning("Could not resolve application as HTTP request is null");
        return null;
      }

      IApplication app;

      if (context.IsBackofficeRequest(Options.BackofficePath))
      {
        app = await ResolveFromUser(user);
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

      return app;
    }


    /// <inheritdoc />
    public async Task<IApplication> ResolveFromUser(ClaimsPrincipal user)
    {
      IBackofficeUser userEntity = await GetBackofficeUser(user);
      return await ResolveFromUser(userEntity);
    }


    /// <inheritdoc />
    public async Task<IApplication> ResolveFromUser(IBackofficeUser user)
    {
      if (user == null)
      {
        return null;
      }

      string appId;
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

      using IAsyncDocumentSession session = Store.OpenCoreSession();
      return await session.LoadAsync<IApplication>(appId);
    }


    /// <inheritdoc />
    public async Task<IApplication> ResolveFromRequest(HttpContext context)
    {
      return Handler.Get<IApplicationResolverHandler>()?.Resolve(context.Request, await GetApplications()) ?? await ResolveFromUri(context.Request.GetEncodedUrl());
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


    /// <summary>
    /// Get matching application from an URI
    /// </summary>
    IApplication ResolveFromUriInternal(Uri uri, IList<IApplication> apps)
    {
      foreach (IApplication app in apps)
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
    async Task<IList<IApplication>> GetApplications()
    {
      if (Apps != null)
      {
        return Apps;
      }

      using IAsyncDocumentSession session = Store.OpenCoreSession();
      Apps = await session.Query<IApplication>().ToListAsync();
      return Apps;
    }


    /// <summary>
    /// Get backoffice user from claims principal
    /// </summary>
    async Task<IBackofficeUser> GetBackofficeUser(ClaimsPrincipal user)
    {
      string userId = user.FindFirstValue(Constants.Auth.Claims.UserId);

      using IAsyncDocumentSession session = Store.OpenCoreSession();
      return await session.LoadAsync<IBackofficeUser>(userId);
    }


    /// <summary>
    /// Get applications the user has access to
    /// </summary>
    string[] GetAllowedAppIdsForUser(IBackofficeUser user)
    {
      IEnumerable<Permission> permissions = user.Claims
        .Where(claim => claim.Type == Constants.Auth.Claims.Permission && claim.Value.StartsWith(Permissions.Applications))
        .Select(x => Permission.FromClaim(x.ToClaim(), Permissions.Applications));

      return permissions.Where(x => x.IsTrue).Select(x => x.NormalizedKey).ToArray();
    }


    /// <inheritdoc />
    //public async Task<bool> TrySwitchForUser(IBackofficeUser user, string appId)
    //{
    //  if (user == null || appId.IsNullOrEmpty())
    //  {
    //    return false;
    //  }

    //  string[] allowedAppIds = GetAllowedAppIdsForUser(user);

    //  bool isMainId = appId.Equals(user.AppId, StringComparison.InvariantCultureIgnoreCase);
    //  bool isAllowedId = allowedAppIds.Contains(appId, StringComparer.InvariantCultureIgnoreCase);

    //  if (user.IsSuper || isMainId || isAllowedId)
    //  {
    //    user.CurrentAppId = appId;

    //    //byte[] bytes = new byte[20];
    //    //RandomNumberGenerator.Fill(bytes);
    //    //user.SecurityStamp = Base32.ToBase32(bytes); // TODO update security stamp but Base32 is .net core internal

    //    using IAsyncDocumentSession session = Store.OpenCoreSession();
    //    await session.StoreAsync(user);
    //    await session.SaveChangesAsync();

    //    return true;
    //  }

    //  return false;
    //}
  }


  public interface IApplicationResolver
  {
    /// <summary>
    /// Resolves the current application from either the backoffice user (in case it is backoffice request)
    /// or the domain (in case it is frontend request).
    /// </summary>
    Task<IApplication> Resolve(HttpContext context, ClaimsPrincipal user);

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
    Task<IApplication> ResolveFromUser(IBackofficeUser user);
  }
}
