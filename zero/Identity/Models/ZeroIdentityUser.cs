using Microsoft.AspNetCore.Identity;

namespace zero.Identity;

public abstract class ZeroIdentityUser : ZeroEntity
{
  /// <summary>
  /// Optional username (can also be used as login when configured)
  /// </summary>
  public string Username { get; set; }

  /// <summary>
  /// E-Mail address which is also used as the username
  /// </summary>
  public string Email { get; set; }

  /// <summary>
  /// Whether the email address has been confirmed
  /// </summary>
  public bool IsEmailConfirmed { get; set; }
  
  /// <summary>
  /// The phone number for the user
  /// </summary>
  public string PhoneNumber { get; set; }

  /// <summary>
  /// Whether the phone number has been confirmed
  /// </summary>
  public bool IsPhoneNumberConfirmed { get; set; }

  /// <summary>
  /// The password hash
  /// </summary>
  public string PasswordHash { get; set; }

  /// <summary>
  /// The security stamp
  /// </summary>
  public string SecurityStamp { get; set; }

  /// <summary>
  /// The user's claims, for use in claims-based authentication.
  /// </summary>
  public List<UserClaim> Claims { get; set; } = new();

  /// <summary>
  /// The roles (aliases) of the user
  /// </summary>
  public List<string> RoleIds { get; set; } = new();



  /// <summary>
  /// Number of times sign in failed.
  /// </summary>
  public int AccessFailedCount { get; set; }

  /// <summary>
  /// Whether the user is locked out.
  /// </summary>
  public bool LockoutEnabled { get; set; }

  /// <summary>
  /// When the user lock out is over.
  /// </summary>
  public DateTimeOffset? LockoutEnd { get; set; }



  /// <summary>
  /// Whether 2-factor authentication is enabled or not
  /// </summary>
  public bool TwoFactorEnabled { get; set; }

  /// <summary>
  /// The two-factor authenticator key
  /// </summary>
  public string TwoFactorAuthenticatorKey { get; set; }

  /// <summary>
  /// The list of two factor authentication recovery codes
  /// </summary>
  public List<string> TwoFactorRecoveryCodes { get; set; } = new();

  /// <summary>
  /// Store all external logins (Microsoft, Google, ...)
  /// </summary>
  public List<UserExternalLogin> ExternalLogins { get; set; } = new();
  
  /// <summary>
  /// Authenticator tokens
  /// </summary>
  public List<UserToken> Tokens { get; set; } = new();
}
