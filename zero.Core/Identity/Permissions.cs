namespace zero.Core.Identity
{
  public struct Permissions
  {
    public struct Settings
    {
      public const string Updates = "settings.area." + Constants.SettingsAreas.Updates;
      public const string Applications = "settings.area." + Constants.SettingsAreas.Applications;
      public const string Users = "settings.area." + Constants.SettingsAreas.Users;
      public const string Translations = "settings.area." + Constants.SettingsAreas.Translations;
      public const string Countries = "settings.area." + Constants.SettingsAreas.Countries;
      public const string Logging = "settings.area." + Constants.SettingsAreas.Logging;
    }

    public struct Sections
    {
      public const string Dashboard = "section." + Constants.Sections.Dashboard;
      public const string Pages = "section." + Constants.Sections.Pages;
      public const string Lists = "section." + Constants.Sections.Lists;
      public const string Media = "section." + Constants.Sections.Media;
      public const string Settings = "section." + Constants.Sections.Settings;
    }
  }


  public struct PermissionsValue
  {
    public const string Read = "read";

    public const string Write = "write";

    public const string None = "none";

    public const string True = "true";

    public const string False = "false";
  }
}
