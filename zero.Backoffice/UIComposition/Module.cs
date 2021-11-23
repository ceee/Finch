using Microsoft.Extensions.DependencyInjection;

namespace zero.Backoffice.UIComposition;

internal class BackofficeUICompositionModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddSingleton<IBackofficeSection, DashboardSection>();
    config.Services.AddSingleton<IBackofficeSection, PagesSection>();
    config.Services.AddSingleton<IBackofficeSection, SpacesSection>();
    config.Services.AddSingleton<IBackofficeSection, MediaSection>();
    config.Services.AddSingleton<IBackofficeSection, SettingsSection>();

    config.Services.AddSingleton<ISettingsGroup, SystemSettings>();
    config.Services.AddSingleton<ISettingsGroup, ApplicationSettings>();

    config.Services.AddScoped<IPermissionProvider, CountryPermissions>();
  }


  /// <inheritdoc />
  public override void Configure(IZeroOptions options)
  {
    
  }
}