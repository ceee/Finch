using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using zero.Core.Entities;
using zero.Core.Options;

namespace zero.Core.Security
{
  public class ConfigureZeroBackofficeCookieOptions<TUser> : IConfigureNamedOptions<CookieAuthenticationOptions> where TUser : class, IUser
  {
    protected IZeroOptions Zero { get; set; }

    protected ISystemClock SystemClock { get; set; }


    public ConfigureZeroBackofficeCookieOptions(IZeroOptions zero, ISystemClock systemClock)
    {
      Zero = zero;
      SystemClock = systemClock;
    }


    public void Configure(string name, CookieAuthenticationOptions options)
    {
      if (name == Constants.Auth.BackofficeScheme)
      {
        Configure(options);
      }
    }

    public void Configure(CookieAuthenticationOptions options)
    {
      options.SlidingExpiration = true;
      options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
      options.Cookie.Name = Constants.Auth.BackofficeCookieName;
      options.Cookie.HttpOnly = true;
      options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
      options.Cookie.Path = "/";

      options.LoginPath = Zero.BackofficePath;
      options.LogoutPath = Zero.BackofficePath;
      options.AccessDeniedPath = Zero.BackofficePath;

      options.CookieManager = new ZeroCookieManager(Zero, true);
      options.Events = new ZeroCookieAuthenticationEvents<TUser>(Zero, SystemClock);
    }
  }
}
