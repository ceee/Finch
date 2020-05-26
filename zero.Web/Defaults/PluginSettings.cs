using zero.Core;
using zero.Core.Entities;

namespace zero.Web.Defaults
{
  public class PluginSettings : SettingsGroup
  {
    public PluginSettings()
    {
      Name = "@settings.groups.plugins";

      AddInternal(Constants.Settings.Plugins, "@settings.plugins.installed.name", "@settings.plugins.installed.text", "fth-package");
      AddInternal(Constants.Settings.CreatePlugin, "@settings.plugins.create.name", "@settings.plugins.create.text", "fth-box");
    }
  }
}
