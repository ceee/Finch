using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using zero.Backoffice.Modules.Countries;

namespace zero.Backoffice.Modules;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroBackofficeModules(this IServiceCollection services, IConfiguration config)
  {
    services.AddScoped<IPageTreeService, PageTreeService>();

    services.AddSingleton<IPermissionProvider, CountryPermissions>();

    services.Configure<RavenOptions>(opts =>
    {
      opts.Indexes.Add<zero_Backoffice_Countries_Listing>();
    });

    return services;
  }
}