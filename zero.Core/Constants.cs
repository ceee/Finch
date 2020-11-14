namespace zero.Core
{
  public static class Constants
  {
    public const string ErrorFieldNone = "__zero_no_field";

    public static class Tabs
    {
      public const string General = "general";
    }

    public static class Auth
    {
      public const string DefaultScheme = "zeroScheme";
      public const string BackofficeDisplayName = "Zero Bacckoffice";
      public const string BackofficeScheme = "zeroBackoffice";
      public const string BackofficeCookieName = "zero.be.session";
      public const string DefaultCookieName = "zero.session";

      public static class Claims
      {
        public const string IsZero = "zero.claim.iszero";
        public const string IsSuper = "zero.claim.issuper";
        public const string UserId = "zero.claim.userid";
        public const string UserName = "zero.claim.username";
        public const string Role = "zero.claim.rolealias";
        public const string SecurityStamp = "zero.claim.securitystamp";
        public const string Email = "zero.claim.email";
        public const string Permission = "zero.claim.permission";
        public const string DefaultAppId = "zero.claim.defaultAppId";
        public const string AppId = "zero.claim.appId"; // TODO appx fix
        public const string TicketExpires = "zero.claim.ticketExpires";
      }
    }

    public static class Database
    {
      public const string ReservationPrefix = "zero.";
      public const string Expires = Raven.Client.Constants.Documents.Metadata.Expires;
    }

    public static class Sections
    {
      public const string Dashboard = "dashboard";
      public const string Pages = "pages";
      public const string Spaces = "spaces";
      public const string Media = "media";
      public const string Settings = "settings";
    }

    public static class Settings
    {
      public const string Updates = "updates";
      public const string Applications = "applications";
      public const string Users = "users";
      public const string Languages = "languages";
      public const string Translations = "translations";
      public const string Countries = "countries";
      public const string Mails = "mailTemplates";
      public const string Logging = "logs";
      public const string Plugins = "plugins";
      public const string CreatePlugin = "createplugin";
    }

    public static class PermissionCollections
    {
      public const string Sections = "permissionCollectionSections";

      public const string Spaces = "permissionCollectionSpaces";

      public const string Settings = "permisssionCollectionSettings";

      public const string Modules = "permissionCollectionModules";
    }
  }
}
