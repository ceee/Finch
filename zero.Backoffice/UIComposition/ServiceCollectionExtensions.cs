using Microsoft.Extensions.DependencyInjection;

namespace zero.Backoffice.UIComposition;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroBackofficeUIComposition(this IServiceCollection services)
  {
    services.AddSingleton<IBackofficeSection, DashboardSection>();
    services.AddSingleton<IBackofficeSection, PagesSection>();
    services.AddSingleton<IBackofficeSection, SpacesSection>();
    services.AddSingleton<IBackofficeSection, MediaSection>();
    services.AddSingleton<IBackofficeSection, SettingsSection>();

    services.AddSingleton<ISettingsGroup, SystemSettings>();
    services.AddSingleton<ISettingsGroup, ApplicationSettings>();

    services.AddScoped<IPermissionProvider, SectionPermissions>();

    services.AddOptions<BackofficeSettingsOptions>();

    return services;
  }
}