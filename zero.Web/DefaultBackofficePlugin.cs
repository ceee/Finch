using zero.Core;
using zero.Core.Entities;
using zero.Core.Plugins;
using zero.Web.Sections;

namespace zero.Web
{
  public class DefaultBackofficePlugin : ZeroPlugin
  {
    public DefaultBackofficePlugin()
    {
      Sections.Add<DashboardSection>();
      Sections.Add<PagesSection>();
      Sections.Add<SpacesSection>();
      Sections.Add<MediaSection>();
      Sections.Add<SettingsSection>();

      SettingsGroup systemSettings = new SettingsGroup("@settings.groups.system");
      systemSettings.Add(Constants.Settings.Updates, "@settings.system.updates.name", "@settings.system.updates.text", "fth-check-circle");
      systemSettings.Add(Constants.Settings.Applications, "@settings.system.applications.name", "@settings.system.applications.text", "fth-layers");
      systemSettings.Add(Constants.Settings.Users, "@settings.system.users.name", "@settings.system.users.text", "fth-users");
      systemSettings.Add(Constants.Settings.Languages, "@settings.system.languages.name", "@settings.system.languages.text", "fth-globe");
      systemSettings.Add(Constants.Settings.Countries, "@settings.system.countries.name", "@settings.system.countries.text", "fth-map-pin");
      systemSettings.Add(Constants.Settings.Translations, "@settings.system.translations.name", "@settings.system.translations.text", "fth-type");
      systemSettings.Add(Constants.Settings.Logging, "@settings.system.logs.name", "@settings.system.logs.text", "fth-file-text");

      SettingsGroup pluginSettings = new SettingsGroup("@settings.groups.plugins");
      pluginSettings.Add(Constants.Settings.Plugins, "@settings.plugins.installed.name", "@settings.plugins.installed.text", "fth-package");
      pluginSettings.Add(Constants.Settings.CreatePlugin, "@settings.plugins.create.name", "@settings.plugins.create.text", "fth-box");

      Settings.Add(systemSettings);
      Settings.Add(pluginSettings);
    }
  }
}
