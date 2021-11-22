using Microsoft.Extensions.DependencyInjection;

namespace zero.Localization;

internal class LocalizationModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddScoped<ICultureResolver, CultureResolver>();
    config.Services.AddScoped<ILocalizer, Localizer>();
  }
}