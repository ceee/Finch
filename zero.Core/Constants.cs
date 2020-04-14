namespace zero.Core
{
  public static class Constants
  {
    public static class Auth
    {
      public const string Scheme = "zeroCookies";

      public const string CookieName = "zero.session";
    }

    public static class Database
    {
      public const string SharedAppId = "shared";

      public const string CollectionPrefix = "zero.";

      public const string ReservationPrefix = "zero.";
    }

    public static class Sections
    {
      public const string Dashboard = "dashboard";

      public const string Pages = "pages";

      public const string Lists = "lists";

      public const string Media = "media";

      public const string Settings = "settings";
    }

    public static class SettingsAreas
    {
      public const string Updates = "updates";

      public const string Applications = "applications";

      public const string Users = "users";

      public const string Translations = "translations";

      public const string Countries = "countries";

      public const string Logging = "logs";
    }
  }
}
