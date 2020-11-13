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
      AddInternal(Constants.Settings.Languages, "@settings.system.languages.name", "@settings.system.languages.text", "fth-globe");
      AddInternal(Constants.Settings.Countries, "@settings.system.countries.name", "@settings.system.countries.text", "fth-map-pin");
      AddInternal(Constants.Settings.Translations, "@settings.system.translations.name", "@settings.system.translations.text", "fth-type");
      AddInternal(Constants.Settings.Mails, "@settings.system.mails.name", "@settings.system.mails.text", "fth-mail");
      //AddInternal(Constants.Settings.Logging, "@settings.system.logs.name", "@settings.system.logs.text", "fth-file-text");
    }
  }
}
