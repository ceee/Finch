using Microsoft.Extensions.DependencyInjection;
using zero.Core;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Web.Sections;

namespace zero.Web.Defaults
{
  public class DefaultBackofficePlugin : IZeroPlugin
  {
    public void Configure(IServiceCollection services, IZeroOptions zero)
    {
      zero.Sections.Add<DashboardSection>();
      zero.Sections.Add<PagesSection>();
      zero.Sections.Add<SpacesSection>();
      zero.Sections.Add<MediaSection>();
      zero.Sections.Add<SettingsSection>();

      zero.Settings.AddGroup<SystemSettings>();
      zero.Settings.AddGroup<PluginSettings>();
    }
  }
}