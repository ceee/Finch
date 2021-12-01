using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Localization;

public class LocalizationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<ICultureResolver, CultureResolver>();
    services.AddScoped<ICultureService, CultureService>();
    services.AddScoped<ICountryStore, CountryStore>();
    services.AddScoped<ILanguageStore, LanguageStore>();
    services.AddScoped<ITranslationStore, TranslationStore>();
    services.AddScoped<ILocalizer, Localizer>();

    services.Configure<FlavorOptions>(opts =>
    {
      opts.Provide<Country>("zero.country", "Country", "Default country", "fth-flag");
      opts.Provide<Language>("zero.language", "Language", "Default language", "fth-flag");
      opts.Provide<Translation>("zero.translation", "Translation", "Default translation", "fth-flag");
    });
  }
}