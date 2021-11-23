namespace zero.Backoffice.UIComposition;

public class SystemSettings : SettingsGroup
{
  public SystemSettings() : base("@settings.groups.system")
  {
    //Add(Constants.Settings.Updates, "@settings.system.updates.name", "@settings.system.updates.text", "fth-check-circle");
    Add(Constants.Settings.Applications, "@settings.system.applications.name", "@settings.system.applications.text", "fth-layers");
    Add(Constants.Settings.Users, "@settings.system.users.name", "@settings.system.users.text", "fth-users");
    Add(Constants.Settings.Plugins, "@settings.system.plugins.name", "@settings.system.plugins.text", "fth-package");
    //Add(Constants.Settings.Logging, "@settings.system.logs.name", "@settings.system.logs.text", "fth-file-text");
  }
}
