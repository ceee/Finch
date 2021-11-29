using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using zero.Api.Modules.Countries;
using zero.Api.Modules.Pages;
using zero.Api.Modules.Search;

namespace zero.Api.Modules;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroBackofficeModules(this IServiceCollection services, IConfiguration config)
  {
    services.AddZeroBackofficeCountries();

    services.AddScoped<IPageTreeService, PageTreeService>();
    services.AddScoped<IBackofficeSearchService, BackofficeSearchService>();

    //services.Configure<RavenOptions>(opts =>
    //{
    //  opts.Indexes.Add<zero_Backoffice_Search>();
    //});

    return services;
  }
}