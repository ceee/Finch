using System;
using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class BackofficeUser : DatabaseEntity, IBackofficeUser
  {
    /// <inheritdoc/>
    public bool IsSuper { get; set; }

    /// <inheritdoc/>
    public string Email { get; set; }

    /// <inheritdoc/>
    public string PasswordHash { get; set; }

    /// <inheritdoc/>
    public string SecurityStamp { get; set; }

    /// <inheritdoc/>
    public Media Avatar { get; set; }

    /// <inheritdoc/>
    public string LanguageId { get; set; }



    /// <inheritdoc/>
    public List<string> RoleIds { get; set; } = new List<string>();

    /// <inheritdoc/>
    public List<IBackofficeUserClaim> Claims { get; set; } = new List<IBackofficeUserClaim>();



    /// <inheritdoc/>
    public string PasswordResetToken { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset? PasswordResetTokenExpirationDate { get; set; }

    /// <inheritdoc/>
    public int FailedPasswordAttempts { get; set; }

    /// <inheritdoc/>
    public bool IsLockedOut { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset? LockoutEndDate { get; set; }



    /// <inheritdoc/>
    public bool TwoFactorEnabled { get; set; }

    /// <inheritdoc/>
    public string TwoFactorAuthenticatorKey { get; set; }

    /// <inheritdoc/>
    public List<string> TwoFactorRecoveryCodes { get; set; } = new List<string>();
  }



  public interface IBackofficeUser : IDatabaseEntity
  {
    /// <summary>
    /// sudo.
    /// The user who created the instance.
    /// </summary>
    bool IsSuper { get; set; }

    /// <summary>
    /// E-Mail address which is also used as the username
    /// </summary>
    string Email { get; set; }

    /// <summary>
    /// The password hash
    /// </summary>
    string PasswordHash { get; set; }

    /// <summary>
    /// The security stamp
    /// </summary>
    string SecurityStamp { get; set; }

    /// <summary>
    /// Avatar image
    /// </summary>
    Media Avatar { get; set; }

    /// <summary>
    /// Backoffice display language
    /// </summary>
    string LanguageId { get; set; }



    /// <summary>
    /// The roles of the user
    /// </summary>
    List<string> RoleIds { get; set; }

    /// <summary>
    /// The user's claims, for use in claims-based authentication.
    /// </summary>
    List<IBackofficeUserClaim> Claims { get; set; }



    /// <summary>
    /// A hash which is used to validate a password-change request
    /// </summary>
    string PasswordResetToken { get; set; }

    /// <summary>
    /// The date when the current password-reset hash expires
    /// </summary>
    DateTimeOffset? PasswordResetTokenExpirationDate { get; set; }

    /// <summary>
    /// Number of times sign in failed.
    /// </summary>
    int FailedPasswordAttempts { get; set; }

    /// <summary>
    /// Whether the user is locked out.
    /// </summary>
    bool IsLockedOut { get; set; }

    /// <summary>
    /// When the user lock out is over.
    /// </summary>
    DateTimeOffset? LockoutEndDate { get; set; }



    /// <summary>
    /// Whether 2-factor authentication is enabled or not
    /// </summary>
    bool TwoFactorEnabled { get; set; }

    /// <summary>
    /// The two-factor authenticator key
    /// </summary>
    string TwoFactorAuthenticatorKey { get; set; }

    /// <summary>
    /// The list of two factor authentication recovery codes
    /// </summary>
    List<string> TwoFactorRecoveryCodes { get; set; }
  }
}
