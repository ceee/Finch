using Microsoft.Extensions.DependencyInjection;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Web.Sections;

namespace zero.Web.Defaults
{
  public class DefaultBackofficePlugin : IZeroPlugin
  {
    public void Configure(IServiceCollection services, IZeroPluginConfiguration zero)
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


      zero.Configure<PermissionOptions>(opts =>
      {

      });
    }
  }
}