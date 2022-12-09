using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Localization;

internal class ZeroLocalizationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    // hint: think about using https://github.com/nuages-io/nuages-localization
    
    services.AddScoped<ICultureResolver, CultureResolver>();
    services.AddScoped<ICultureService, CultureService>();
    services.AddScoped<ILocalizer, FileLocalizer>();
    
    services.AddOptions<LocalizationOptions>().Bind(configuration.GetSection("Zero:Localization")).Configure(opts =>
    {
      opts.FilePath = "Config/texts.json";
    });
  }
}