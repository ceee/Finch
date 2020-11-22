using System;
using System.Collections.Generic;
using zero.Core.Attributes;
using zero.Core.Identity;

namespace zero.Core.Entities
{
  public class BackofficeUser : ZeroEntity, IBackofficeUser
  {
    /// <inheritdoc />
    public string Username { get; set; }

    /// <inheritdoc />
    public string AppId { get; set; }

    /// <inheritdoc />
    public string CurrentAppId { get; set; }

    /// <inheritdoc/>
    public bool IsSuper { get; set; }

    /// <inheritdoc/>
    public string Email { get; set; }

    /// <inheritdoc/>
    public bool IsEmailConfirmed { get; set; }

    /// <inheritdoc/>
    public string PasswordHash { get; set; }

    /// <inheritdoc/>
    public string SecurityStamp { get; set; }

    /// <inheritdoc/>
    public MediaRef AvatarId { get; set; }

    /// <inheritdoc/>
    public string LanguageId { get; set; }



    /// <inheritdoc/>
    public List<string> RoleIds { get; set; } = new List<string>();

    /// <inheritdoc/>
    public List<IUserClaim> Claims { get; set; } = new List<IUserClaim>();



    /// <inheritdoc/>
    public string PasswordResetToken { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset? PasswordResetTokenExpirationDate { get; set; }



    /// <inheritdoc/>
    public int AccessFailedCount { get; set; }

    /// <inheritdoc/>
    public bool LockoutEnabled { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset? LockoutEnd { get; set; }



    ///// <inheritdoc/>
    //public bool TwoFactorEnabled { get; set; }

    ///// <inheritdoc/>
    //public string TwoFactorAuthenticatorKey { get; set; }

    ///// <inheritdoc/>
    //public List<string> TwoFactorRecoveryCodes { get; set; } = new List<string>();
  }


  [Collection("BackofficeUsers")]
  public interface IBackofficeUser : IZeroEntity, IZeroDbConventions, IIdentityUserWithRoles
  {
    /// <summary>
    /// Application the user registered in
    /// </summary>
    string AppId { get; set; }

    /// <summary>
    /// Currently selected app id for the backoffice
    /// </summary>
    string CurrentAppId { get; set; }

    /// <summary>
    /// sudo.
    /// The user who created the instance.
    /// </summary>
    bool IsSuper { get; set; }

    /// <summary>
    /// Avatar image
    /// </summary>
    MediaRef AvatarId { get; set; }

    /// <summary>
    /// Backoffice display language
    /// </summary>
    string LanguageId { get; set; }
  }
}
