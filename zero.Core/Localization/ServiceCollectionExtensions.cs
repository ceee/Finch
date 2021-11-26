using Microsoft.Extensions.DependencyInjection;

namespace zero.Localization;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroLocalization(this IServiceCollection services)
  {
    services.AddScoped<ICultureResolver, CultureResolver>();
    services.AddScoped<ICultureService, CultureService>();
    services.AddScoped<ICountryStore, CountryStore>();
    services.AddScoped<ILanguageStore, LanguageStore>();
    services.AddScoped<ITranslationStore, TranslationStore>();
    services.AddScoped<ILocalizer, Localizer>();
    return services;
  }
}