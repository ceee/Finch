using zero.Core;
using zero.Core.Entities;

namespace zero.Web.Settings
{
  public class PluginSettings : SettingsGroup
  {
    public PluginSettings()
    {
      Name = "@settings.groups.plugins";

      Add(Constants.Settings.Plugins, "@settings.plugins.installed.name", "@settings.plugins.installed.text", "fth-package");
      Add(Constants.Settings.CreatePlugin, "@settings.plugins.create.name", "@settings.plugins.create.text", "fth-box");
    }
  }
}
