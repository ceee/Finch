namespace zero.Configuration;

public static partial class Constants
{
  public const string ErrorFieldNone = "__zero_no_field";

  public static partial class Auth
  {
    public const string SystemUser = "system";
    public const string DefaultScheme = "zeroScheme";
    public const string BackofficeDisplayName = "Zero Backoffice";
    public const string BackofficeScheme = "zeroBackoffice";
    public const string BackofficeCookieName = "zero.be.session";
    public const string DefaultCookieName = "zero.session";
  }

  public static partial class Database
  {
    public const string ReservationPrefix = "zero.";
  }
}
