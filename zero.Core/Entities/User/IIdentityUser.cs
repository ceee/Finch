using System;
using System.Collections.Generic;

namespace zero.Core.Entities
{
  public interface IIdentityUserWithRoles : IIdentityUser
  {
    /// <summary>
    /// The roles (aliases) of the user
    /// </summary>
    List<string> Roles { get; set; }
  }


  public interface IIdentityUser : IAppAwareEntity, IZeroEntity
  {
    /// <summary>
    /// Optional username (can also be used as login when configured)
    /// </summary>
    string Username { get; set; }

    /// <summary>
    /// E-Mail address which is also used as the username
    /// </summary>
    string Email { get; set; }

    /// <summary>
    /// Whether the email address has been confirmed
    /// </summary>
    public bool IsEmailConfirmed { get; set; }

    /// <summary>
    /// The password hash
    /// </summary>
    string PasswordHash { get; set; }

    /// <summary>
    /// The security stamp
    /// </summary>
    string SecurityStamp { get; set; }

    /// <summary>
    /// The user's claims, for use in claims-based authentication.
    /// </summary>
    List<IUserClaim> Claims { get; set; }



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
    int AccessFailedCount { get; set; }

    /// <summary>
    /// Whether the user is locked out.
    /// </summary>
    bool LockoutEnabled { get; set; }

    /// <summary>
    /// When the user lock out is over.
    /// </summary>
    DateTimeOffset? LockoutEnd { get; set; }



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
