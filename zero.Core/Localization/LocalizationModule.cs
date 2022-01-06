using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Localization;

internal class LocalizationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<ICultureResolver, CultureResolver>();
    services.AddScoped<ICultureService, CultureService>();
    services.AddScoped<ICountryStore, CountryStore>();
    services.AddScoped<ILanguageStore, LanguageStore>();
    services.AddScoped<ITranslationStore, TranslationStore>();
    services.AddScoped<ILocalizer, Localizer>();
  }
}