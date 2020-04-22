namespace zero.Core
{
  public static class Constants
  {
    public const string ErrorFieldNone = "__zero_no_field";

    public static class Auth
    {
      public const string Scheme = "zeroCookies";
      public const string CookieName = "zero.session";

      public static class Claims
      {
        public const string IsZero = "zero.claim.iszero";
        public const string IsSuper = "zero.claim.issuper";
        public const string UserId = "zero.claim.userid";
        public const string UserName = "zero.claim.username";
        public const string Role = "zero.claim.rolealias";
        public const string SecurityStamp = "zero.claim.securitystamp";
        public const string Permission = "zero.claim.permission";
      }
    }

    public static class Database
    {
      public const string SharedAppId = "shared";
      public const string CollectionPrefix = "zero.";
      public const string ReservationPrefix = "zero.";
      public const string Expires = Raven.Client.Constants.Documents.Metadata.Expires;
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
