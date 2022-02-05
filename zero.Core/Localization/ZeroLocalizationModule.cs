using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Localization;

internal class ZeroLocalizationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<ICultureResolver, CultureResolver>();
    services.AddScoped<ICultureService, CultureService>();
    services.AddScoped<ICountryStore, CountryStore>();
    services.AddScoped<ILanguageStore, LanguageStore>();
    services.AddScoped<ITranslationStore, TranslationStore>();
    services.AddScoped<ILocalizer, Localizer>();

    services.AddScoped<IValidator<Country>, CountryValidator>();
    services.AddScoped<IValidator<Language>, LanguageValidator>();
    services.AddScoped<IValidator<Translation>, TranslationValidator>();

    services.Configure<ZeroSearchOptions>(opts =>
    {
      opts.Map<Country>("fth-map-pin").Fields().Display((x, res) =>
      {
        res.Url = "/settings/countries/edit/" + x.Id;
      });
      opts.Map<Language>("fth-globe").Fields().Display((x, res) =>
      {
        res.Url = "/settings/languages/edit/" + x.Id;
      });
      opts.Map<Translation>("fth-type").Fields("Value").Display((x, res) =>
      {
        res.Description = x.Value;
        res.Url = "/settings/translations/edit/" + x.Id;
      });
    });
  }
}