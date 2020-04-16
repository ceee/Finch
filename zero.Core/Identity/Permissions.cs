namespace zero.Core.Identity
{
  public struct Permissions
  {
    /// <summary>
    /// Ability to switch zero applications and read/write other applications out of the user appId
    /// If PermissionsValue.Write is set for this permission, the user will be limited to his/her claims in other applications too
    /// </summary>
    public const string Applications = "applications";


    public struct Settings
    {
      public const string PREFIX = "settings.area.";
      public const string Updates = PREFIX + Constants.SettingsAreas.Updates;
      public const string Applications = PREFIX + Constants.SettingsAreas.Applications;
      public const string Users = PREFIX + Constants.SettingsAreas.Users;
      public const string Translations = PREFIX + Constants.SettingsAreas.Translations;
      public const string Countries = PREFIX + Constants.SettingsAreas.Countries;
      public const string Logging = PREFIX + Constants.SettingsAreas.Logging;
    }


    public struct Sections
    {
      public const string PREFIX = "section.";
      public const string Dashboard = PREFIX + Constants.Sections.Dashboard;
      public const string Pages = PREFIX + Constants.Sections.Pages;
      public const string Lists = PREFIX + Constants.Sections.Lists;
      public const string Media = PREFIX + Constants.Sections.Media;
      public const string Settings = PREFIX + Constants.Sections.Settings;
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
