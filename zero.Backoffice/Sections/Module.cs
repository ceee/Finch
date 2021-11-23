using Microsoft.Extensions.DependencyInjection;

namespace zero.Backoffice.Sections;

internal class BackofficeSectionModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddSingleton<IBackofficeSection, DashboardSection>();
    config.Services.AddSingleton<IBackofficeSection, PagesSection>();
    config.Services.AddSingleton<IBackofficeSection, SpacesSection>();
    config.Services.AddSingleton<IBackofficeSection, MediaSection>();
    config.Services.AddSingleton<IBackofficeSection, SettingsSection>();
    config.Services.AddScoped<IPermissionProvider, CountryPermissions>();
  }


  /// <inheritdoc />
  public override void Configure(IZeroOptions options)
  {
    
  }
}