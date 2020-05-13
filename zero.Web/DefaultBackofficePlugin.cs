using Microsoft.Extensions.DependencyInjection;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Web.Sections;
using zero.Web.Settings;

namespace zero.Web
{
  public class DefaultBackofficePlugin : IZeroPlugin
  {
    public void Configure(IServiceCollection services, IZeroPluginBuilder zero)
    {
      zero.Configure<SectionOptions>(opts =>
      {
        opts.Add<DashboardSection>();
        opts.Add<PagesSection>();
        opts.Add<SpacesSection>();
        opts.Add<MediaSection>();
        opts.Add<SettingsSection>();
      });

      zero.Configure<SettingsOptions>(opts =>
      {
        opts.AddGroup<SystemSettings>();
        opts.AddGroup<PluginSettings>();
      });
    }
  }
}
