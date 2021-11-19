using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace zero;

internal class IdentityModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>());

    config.Services.AddZeroIdentity<BackofficeUser, BackofficeUserRole>();
    config.Services.Replace<IUserClaimsPrincipalFactory<BackofficeUser>, ZeroBackofficeClaimsPrincipalFactory<BackofficeUser, BackofficeUserRole>>();
    config.Services.Replace<IUserStore<BackofficeUser>, RavenCoreUserStore<BackofficeUser, BackofficeUserRole>>(ServiceLifetime.Scoped);
    config.Services.Replace<IRoleStore<BackofficeUserRole>, RavenCoreRoleStore<BackofficeUserRole>>(ServiceLifetime.Scoped);

    config.Services.AddAuthentication(Constants.Auth.BackofficeScheme);
      //.AddZeroBackofficeCookie<BackofficeUser, BackofficeUserRole>(); // TODO

    config.Services.AddAuthorization();
  }

  /// <inheritdoc />
  public override void Configure(IZeroOptions options)
  {
    
  }
}