using zero.Core;
using zero.Core.Entities;

namespace zero.Web.Defaults
{
  public class SystemSettings : SettingsGroup
  {
    public SystemSettings()
    {
      Name = "@settings.groups.system";

      //AddInternal(Constants.Settings.Updates, "@settings.system.updates.name", "@settings.system.updates.text", "fth-check-circle");
      AddInternal(Constants.Settings.Applications, "@settings.system.applications.name", "@settings.system.applications.text", "fth-layers");
      AddInternal(Constants.Settings.Users, "@settings.system.users.name", "@settings.system.users.text", "fth-users");
      AddInternal(Constants.Settings.Plugins, "@settings.system.plugins.name", "@settings.system.plugins.text", "fth-package");
      //AddInternal(Constants.Settings.Logging, "@settings.system.logs.name", "@settings.system.logs.text", "fth-file-text");
    }
  }
}
