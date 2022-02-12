using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace zero.Identity;

internal class ZeroIdentityModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>());

    services.AddZeroIdentity<ZeroUser, ZeroUserRole>();
    services.Replace<IUserClaimsPrincipalFactory<ZeroUser>, ZeroBackofficeClaimsPrincipalFactory<ZeroUser, ZeroUserRole>>();
    services.Replace<IUserStore<ZeroUser>, RavenCoreUserStore<ZeroUser, ZeroUserRole>>(ServiceLifetime.Scoped);
    services.AddScoped<IValidator<ZeroUser>, BackofficeUserValidator>();
    services.Replace<IRoleStore<ZeroUserRole>, RavenCoreRoleStore<ZeroUserRole>>(ServiceLifetime.Scoped);
    services.AddSingleton<IAuthenticationSchemeProvider, ZeroAuthenticationSchemeProvider>();

    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddScoped<IAuthorizationService, AuthorizationService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IUserRolesService, UserRolesService>();

    services.AddAuthentication(Constants.Auth.BackofficeScheme)
      .AddZeroBackofficeCookie<ZeroUser, ZeroUserRole>();

    services.AddAuthorization();

    services.Configure<ZeroSearchOptions>(opts =>
    {
      opts.Map<ZeroUser>("fth-user-check").Boost(5).Display((x, res) =>
      {
        res.Description = x.Email;
        res.Url = "/settings/users/edit/" + x.Id;
      });
    });
  }
}