namespace zero.Identity;

/// <summary>
/// Represents all the options you can use to configure the cookies middleware used by the identity system.
/// </summary>
public class ZeroIdentityConstants
{
  public static class CookieNames
  {
    private const string CookiePrefix = "zero.id";
    
    public static readonly string Application = CookiePrefix + ".app";
    public static readonly string External = CookiePrefix + ".ext";
    public static readonly string TwoFactorRememberMe = CookiePrefix + ".2fa_rem";
    public static readonly string TwoFactorUserId = CookiePrefix + ".2fa_id";
  }

  public static partial class Claims
  {
    private const string ClaimPrefix = "zero.claim";
    
    public static readonly string IsZero = ClaimPrefix + ".iszero";
    public static readonly string UserId = ClaimPrefix + ".userid";
    public static readonly string UserName = ClaimPrefix + ".username";
    public static readonly string Role = ClaimPrefix + ".rolealias";
    public static readonly string SecurityStamp = ClaimPrefix + ".securitystamp";
    public static readonly string Email = ClaimPrefix + ".email";
    public static readonly string Permission = ClaimPrefix + ".permission";
    public static readonly string TicketExpires = ClaimPrefix + ".ticketExpires";
  }
}
