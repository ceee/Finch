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
    services.AddScoped<ILocalizer, Localizer>();
  }
}