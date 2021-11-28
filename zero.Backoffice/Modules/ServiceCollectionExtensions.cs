using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using zero.Backoffice.Modules.Countries;
using zero.Backoffice.Modules.Pages;
using zero.Backoffice.Modules.Search;

namespace zero.Backoffice.Modules;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroBackofficeModules(this IServiceCollection services, IConfiguration config)
  {
    services.AddScoped<IPageTreeService, PageTreeService>();
    services.AddScoped<IBackofficeSearchService, BackofficeSearchService>();

    services.AddScoped<IPickerProvider<Country>, CountryPickerProvider>();
    services.AddSingleton<IPermissionProvider, CountryPermissions>();

    services.Configure<RavenOptions>(opts =>
    {
      opts.Indexes.Add<zero_Backoffice_Countries_Listing>();
      opts.Indexes.Add<zero_Backoffice_Search>();
    });

    services.Configure<BackofficeOptions>(opts =>
    {
      opts.Mapper.Configure(maps =>
      {
        maps.AddProfile<CountryMapperProfile>();
      });
    });

    return services;
  }
}