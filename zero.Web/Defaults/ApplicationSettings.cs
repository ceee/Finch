using zero.Core;
using zero.Core.Entities;

namespace zero.Web.Defaults
{
  public class ApplicationSettings : SettingsGroup
  {
    public ApplicationSettings()
    {
      Name = "@settings.groups.application";

      //AddInternal(Constants.Settings.Updates, "@settings.system.updates.name", "@settings.system.updates.text", "fth-check-circle");
      AddInternal(Constants.Settings.Languages, "@settings.application.languages.name", "@settings.application.languages.text", "fth-globe");
      AddInternal(Constants.Settings.Countries, "@settings.application.countries.name", "@settings.application.countries.text", "fth-map-pin");
      AddInternal(Constants.Settings.Translations, "@settings.application.translations.name", "@settings.application.translations.text", "fth-type");
      AddInternal(Constants.Settings.Mails, "@settings.application.mails.name", "@settings.application.mails.text", "fth-mail");
      AddInternal(Constants.Settings.Integrations, "@settings.application.integrations.name", "@settings.application.integrations.text", "fth-sliders");
      //AddInternal(Constants.Settings.Logging, "@settings.system.logs.name", "@settings.system.logs.text", "fth-file-text");
    }
  }
}
