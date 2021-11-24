using Microsoft.Extensions.DependencyInjection;

namespace zero.Localization;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroLocalization(this IServiceCollection services)
  {
    services.AddScoped<ICultureResolver, CultureResolver>();
    services.AddScoped<ICultureService, CultureService>();
    services.AddScoped<ICountriesStore, CountriesStore>();
    services.AddScoped<ILanguagesStore, LanguagesStore>();
    services.AddScoped<ILocalizer, Localizer>();
    return services;
  }
}