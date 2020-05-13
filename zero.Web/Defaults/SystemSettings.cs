using zero.Core;
using zero.Core.Entities;

namespace zero.Web.Defaults
{
  public class SystemSettings : SettingsGroup
  {
    public SystemSettings()
    {
      Name = "@settings.groups.system";

      Add(Constants.Settings.Updates, "@settings.system.updates.name", "@settings.system.updates.text", "fth-check-circle");
      Add(Constants.Settings.Applications, "@settings.system.applications.name", "@settings.system.applications.text", "fth-layers");
      Add(Constants.Settings.Users, "@settings.system.users.name", "@settings.system.users.text", "fth-users");
      Add(Constants.Settings.Languages, "@settings.system.languages.name", "@settings.system.languages.text", "fth-globe");
      Add(Constants.Settings.Countries, "@settings.system.countries.name", "@settings.system.countries.text", "fth-map-pin");
      Add(Constants.Settings.Translations, "@settings.system.translations.name", "@settings.system.translations.text", "fth-type");
      Add(Constants.Settings.Logging, "@settings.system.logs.name", "@settings.system.logs.text", "fth-file-text");
    }
  }
}
