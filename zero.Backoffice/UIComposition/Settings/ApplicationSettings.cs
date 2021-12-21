namespace zero.Backoffice.UIComposition;

public class ApplicationSettings : SettingsGroup
{
  public ApplicationSettings() : base("@settings.groups.application")
  {
    Add(Constants.Settings.Languages, "@settings.application.languages.name", "@settings.application.languages.text", "fth-globe");
    Add(Constants.Settings.Countries, "@settings.application.countries.name", "@settings.application.countries.text", "fth-map-pin");
    Add(Constants.Settings.Translations, "@settings.application.translations.name", "@settings.application.translations.text", "fth-type");
    Add(Constants.Settings.Mails, "@settings.application.mails.name", "@settings.application.mails.text", "fth-mail");
    Add(Constants.Settings.Integrations, "@settings.application.integrations.name", "@settings.application.integrations.text", "fth-sliders");
  }
}