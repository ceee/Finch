using Microsoft.Extensions.DependencyInjection;

namespace zero.Api.Modules.Countries;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroBackofficeCountries(this IServiceCollection services)
  {
    services.AddSingleton<IPermissionProvider, CountryPermissions>();
    services.AddSingleton<IMapperProfile, CountryMapperProfile>();

    services.Configure<RavenOptions>(opts =>
    {
      opts.Indexes.Add<zero_Backoffice_Countries_Listing>();
    });

    return services;
  }
}