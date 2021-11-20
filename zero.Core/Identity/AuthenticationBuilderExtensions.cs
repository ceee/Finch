using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace zero.Identity;

public static class AuthenticationBuilderExtensions
{
  public static AuthenticationBuilder AddZeroBackofficeCookie<TUser, TRole>(this AuthenticationBuilder builder, Action<OptionsBuilder<CookieAuthenticationOptions>> setupAction = null)
    where TUser : BackofficeUser
    where TRole : BackofficeUserRole
  {
    return builder.AddCookie<TUser>(Constants.Auth.BackofficeScheme, Constants.Auth.BackofficeDisplayName, true, b =>
    {
      b.Configure<IZeroOptions>((options, zero) =>
      {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(90);
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.Name = Constants.Auth.BackofficeCookieName;

        options.CookieManager = new ContextualCookieManager((ctx, key) =>
        {
          return ctx.Request.Path.ToString().StartsWith(zero.BackofficePath.EnsureStartsWith('/').TrimEnd('/'));
        });

        options.Events.OnRedirectToLogin = ctx =>
        {
          ctx.Response.StatusCode = 401;
          return Task.CompletedTask;
        };
      });
        
      if (setupAction != null)
      {
        setupAction(b);
      }
    });
  }



  public static AuthenticationBuilder AddZeroCookie<TUser, TRole>(this AuthenticationBuilder builder, string scheme, string cookieDisplayName = null, Action<OptionsBuilder<CookieAuthenticationOptions>> setupAction = null)
    where TUser : ZeroIdentityUser
    where TRole : ZeroIdentityRole
  {
    return builder.AddCookie<TUser>(scheme, cookieDisplayName, false, setupAction);
  }



  public static AuthenticationBuilder AddZeroCookie<TUser>(this AuthenticationBuilder builder, string scheme, string cookieDisplayName = null, Action<OptionsBuilder<CookieAuthenticationOptions>> setupAction = null)
    where TUser : ZeroIdentityUser
  {
    return builder.AddCookie<TUser>(scheme, cookieDisplayName, false, setupAction);
  }



  static AuthenticationBuilder AddCookie<TUser>(this AuthenticationBuilder builder, string scheme, string displayName, bool isBackoffice, Action<OptionsBuilder<CookieAuthenticationOptions>> setupAction = null)
    where TUser : ZeroIdentityUser
  {
    IServiceCollection services = builder.Services;

    services
      .AddOptions<ZeroAuthOptions<TUser>>()
      .Configure<IZeroOptions>((options, zero) =>
      {
        options.Scheme = scheme;
        options.Path = isBackoffice ? zero.BackofficePath : "/";
      });

    var optionsBuilder = services
      .AddOptions<CookieAuthenticationOptions>(scheme)
      .Configure<IOptionsMonitor<ZeroAuthOptions<TUser>>>((options, monitor) =>
      {
        ZeroAuthOptions<TUser> opts = monitor.Get(scheme);

        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(90);
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.Path = "/";

        options.LoginPath = opts.Path;
        options.LogoutPath = opts.Path;
        options.AccessDeniedPath = opts.Path;
      });

    if (setupAction != null)
    {
      setupAction(optionsBuilder);
    }

    builder.AddScheme<CookieAuthenticationOptions, CookieAuthenticationHandler>(scheme, displayName, null);

    return builder;
  }
}