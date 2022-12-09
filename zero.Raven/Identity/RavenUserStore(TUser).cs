using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Raven.Client.Exceptions;
using zero.Identity;
using zero.Raven;

namespace zero.Raven;


public partial class RavenUserStore<TUser> :
  IUserStore<TUser>,
  IUserEmailStore<TUser>,
  IUserLockoutStore<TUser>,
  IUserPasswordStore<TUser>,
  IUserClaimStore<TUser>,
  IUserSecurityStampStore<TUser>,
  IUserLoginStore<TUser>,
  IUserAuthenticationTokenStore<TUser>,
  IUserAuthenticatorKeyStore<TUser>,
  IUserTwoFactorStore<TUser>,
  IUserTwoFactorRecoveryCodeStore<TUser>,
  IUserPhoneNumberStore<TUser>
  where TUser : ZeroIdentityUser, new()
{
  public RavenUserStore(IRavenOperations operations, IdentityErrorDescriber describer = null)
  {
    Ops = operations;
    ErrorDescriber = describer ?? new IdentityErrorDescriber();
  }
  
  private StringComparison _comparer = StringComparison.InvariantCultureIgnoreCase;
  private const string InternalLoginProvider = "[AspNetUserStore]";
  private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
  private const string RecoveryCodeTokenName = "RecoveryCodes";
  
  protected IdentityErrorDescriber ErrorDescriber { get; private set; }
  
  protected virtual IRavenOperations Ops { get; set; }
  
  private IdentityResult Fail(Result<TUser> result)
  {
    IdentityError[] errors = new IdentityError[result.Errors.Count];

    int index = 0;
    foreach (ResultError error in result.Errors)
    {
      string message = error.Message + "(key: " + error.Property + ")";
      errors[index++] = new() { Code = "zero/raven/500", Description = message };
    }

    return IdentityResult.Failed(errors);
  }

  /// <summary>
  /// Get the Raven compare/exchange key for this user.
  /// This should be unique within the store, otherwise you can't create users with the same email in different stores.
  /// </summary>
  protected virtual string GetReservationKey(TUser user) => Constants.Database.ReservationPrefix + typeof(TUser) + ':' + user.Email;

  /// <summary>
  /// Whether an email is already reserved
  /// </summary>
  protected virtual async Task<bool> IsEmailReserved(TUser user, CancellationToken cancellationToken = default)
  {
    // TODO index
    TUser existingUser = await Ops.Session.Query<TUser>().FirstOrDefaultAsync(x => x.Email == user.Email, cancellationToken);
    return existingUser != null && existingUser.Id != user.Id;
  }


  /// <inheritdoc />
  public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
  {
    if (await IsEmailReserved(user, cancellationToken))
    {
      return IdentityResult.Failed(new IdentityError
      {
        Code = "DuplicateEmail",
        Description = $"The email address {user.Email} is already taken."
      });
    }

    Result<TUser> result = await Ops.Create(user);
    
    if (!result.IsSuccess)
    {
      return Fail(result);
    }
    
    return IdentityResult.Success;
  }


  /// <inheritdoc />
  public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
  {
    try
    {
      Result<TUser> result = await Ops.Delete(user);
      
      if (!result.IsSuccess)
      {
        return Fail(result);
      }
    }
    catch (ConcurrencyException)
    {
      return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
    }
    return IdentityResult.Success;
  }


  /// <inheritdoc />
  public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
  {
    TUser source = await Ops.Load<TUser>(user.Id);

    if (source == null)
    {
      return IdentityResult.Failed(new IdentityError
      {
        Code = "UserNotFound",
        Description = $"Could not find stored user with id {user.Id}."
      });
    }

    if (source.Email != user.Email && await IsEmailReserved(user, cancellationToken))
    {
      return IdentityResult.Failed(new IdentityError
      {
        Code = "DuplicateEmail",
        Description = $"The email address {user.Email} is already taken."
      });
    }

    Result<TUser> result = await Ops.Update(user);
    
    if (!result.IsSuccess)
    {
      return Fail(result);
    }

    return IdentityResult.Success;
  }


  /// <inheritdoc />
  public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
  {
    // TODO index
    return await Ops.Session.Query<TUser>().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
  }


  /// <inheritdoc />
  public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
  {
    // TODO index
    return await Ops.Session.Query<TUser>().FirstOrDefaultAsync(x => x.Username == normalizedUserName, cancellationToken);
  }


  /// <inheritdoc />
  public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.Username);


  /// <inheritdoc />
  public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.Id);


  /// <inheritdoc />
  public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.Username);


  /// <inheritdoc />
  public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
  {
    user.Username = normalizedName;
    return Task.CompletedTask;
  }


  /// <inheritdoc />
  public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken) =>
    SetNormalizedUserNameAsync(user, userName, cancellationToken);


  /// <inheritdoc />
  void IDisposable.Dispose() { }


  /*
    * ****************************************************
    * SECURITY STAMP
    * ****************************************************
    */


  /// <inheritdoc />
  public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken) =>
    Task.FromResult(user.SecurityStamp);


  /// <inheritdoc />
  public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
  {
    user.SecurityStamp = stamp;
    return Task.CompletedTask;
  }


  /*
    * ****************************************************
    * EMAIL
    * ****************************************************
    */


  /// <inheritdoc />
  public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
  {
    // TODO index
    return await Ops.Session.Query<TUser>().FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);
  }


  /// <inheritdoc />
  public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.Email);


  /// <inheritdoc />
  public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.IsEmailConfirmed);


  /// <inheritdoc />
  public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.Email);


  /// <inheritdoc />
  public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
  {
    user.Email = email.ToLowerInvariant();
    return Task.CompletedTask;
  }


  /// <inheritdoc />
  public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
  {
    user.IsEmailConfirmed = confirmed;
    return Task.CompletedTask;
  }


  /// <inheritdoc />
  public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
  {
    user.Email = normalizedEmail.ToLowerInvariant();
    return Task.CompletedTask;
  }


  /*
    * ****************************************************
    * LOCKOUT
    * ****************************************************
    */


  /// <inheritdoc />
  public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.AccessFailedCount);


  /// <inheritdoc />
  public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.LockoutEnabled);


  /// <inheritdoc />
  public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.LockoutEnd);


  /// <inheritdoc />
  public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
  {
    user.AccessFailedCount += 1;
    return Task.FromResult(user.AccessFailedCount);
  }


  /// <inheritdoc />
  public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
  {
    user.AccessFailedCount = 0;
    return Task.CompletedTask;
  }


  /// <inheritdoc />
  public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
  {
    user.LockoutEnabled = enabled;
    return Task.CompletedTask;
  }


  /// <inheritdoc />
  public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
  {
    user.LockoutEnd = lockoutEnd;
    return Task.CompletedTask;
  }


  /*
    * ****************************************************
    * PASSWORD
    * ****************************************************
    */


  /// <inheritdoc />
  public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.PasswordHash);


  /// <inheritdoc />
  public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(!user.PasswordHash.IsNullOrEmpty());


  /// <inheritdoc />
  public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
  {
    user.PasswordHash = passwordHash;
    return Task.CompletedTask;
  }


  /*
    * ****************************************************
    * CLAIM
    * ****************************************************
    */


  /// <inheritdoc />
  public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
  {
    user.Claims.AddRange(claims.Select(claim => new UserClaim(claim)));
    return Task.CompletedTask;
  }


  /// <inheritdoc />
  public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
  {
    return Task.FromResult((IList<Claim>)user.Claims.Select(claim => claim.ToClaim()).ToList());
  }


  /// <inheritdoc />
  public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
  {
    UserClaim userClaim = new(claim);
    // TODO index
    return await Ops.Session.Query<TUser>().Where(x => x.Claims.Any(c => c.Type == userClaim.Type && c.Value == userClaim.Value)).ToListAsync(token: cancellationToken);
  }


  /// <inheritdoc />
  public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
  {
    IEnumerable<UserClaim> userClaims = claims.Select(c => new UserClaim(c)).ToList();

    user.Claims = user.Claims.Except(userClaims, new UserClaimComparer()).ToList();
    return Task.CompletedTask;
  }


  /// <inheritdoc />
  public Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
  {
    UserClaim userClaim = new(claim);
    UserClaim newUserClaim = new(newClaim);

    user.Claims.Remove(userClaim);
    user.Claims.Add(newUserClaim);
    
    return Task.CompletedTask;
  }

  
  /// <inheritdoc />
  public Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
  {
    user.ExternalLogins.Add(new UserExternalLogin()
    {
      LoginProvider = login.LoginProvider,
      ProviderKey = login.ProviderKey,
      ProviderDisplayName = login.ProviderDisplayName
    });

    return Task.CompletedTask;
  }

  
  /// <inheritdoc />
  public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
  {
    UserExternalLogin login = user.ExternalLogins.FirstOrDefault(x =>
      x.LoginProvider.Equals(loginProvider, _comparer) && x.ProviderKey.Equals(providerKey, _comparer));

    if (login != null)
    {
      user.ExternalLogins.Remove(login);
    }
    
    return Task.CompletedTask;
  }

  
  /// <inheritdoc />
  public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
  {
    return Task.FromResult<IList<UserLoginInfo>>(user.ExternalLogins.Select(x => x.ToLoginInfo()).ToList());
  }

  
  /// <inheritdoc />
  public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
  {
    // TODO index
    return await Ops.Session.Query<TUser>().FirstOrDefaultAsync(x => x.ExternalLogins.Any(l => l.LoginProvider.Equals(loginProvider, _comparer) && l.ProviderKey.Equals(providerKey, _comparer)), token: cancellationToken);
  }

  
  /// <inheritdoc />
  public Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
  {
    UserToken token = user.Tokens.FirstOrDefault(x =>
                        x.LoginProvider.Equals(loginProvider, _comparer) && x.Name.Equals(name, _comparer)) 
                        ?? new() { LoginProvider = loginProvider, Name = name };

    token.Value = value;

    return Task.CompletedTask;
  }

  
  /// <inheritdoc />
  public Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
  {
    UserToken token = user.Tokens.FirstOrDefault(x =>
      x.LoginProvider.Equals(loginProvider, _comparer) && x.Name.Equals(name, _comparer));

    if (token != null)
    {
      user.Tokens.Remove(token);
    }
    
    return Task.CompletedTask;
  }

  
  /// <inheritdoc />
  public Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
  {
    return Task.FromResult(user.Tokens.FirstOrDefault(x =>
      x.LoginProvider.Equals(loginProvider, _comparer) && x.Name.Equals(name, _comparer))?.Value);
  }


  /// <inheritdoc />
  public Task SetAuthenticatorKeyAsync(TUser user, string key, CancellationToken cancellationToken) =>
    SetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);


  /// <inheritdoc />
  public Task<string> GetAuthenticatorKeyAsync(TUser user, CancellationToken cancellationToken) =>
    GetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);

  
  /// <inheritdoc />
  public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
  {
    user.TwoFactorEnabled = enabled;
    return Task.CompletedTask;
  }


  /// <inheritdoc />
  public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken) =>
    Task.FromResult(user.TwoFactorEnabled);


  /// <inheritdoc />
  public Task ReplaceCodesAsync(TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken) =>
    SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, string.Join(";", recoveryCodes), cancellationToken);


  /// <inheritdoc />
  public async Task<bool> RedeemCodeAsync(TUser user, string code, CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();

    if (user == null)
    {
      throw new ArgumentNullException(nameof(user));
    }
    if (code == null)
    {
      throw new ArgumentNullException(nameof(code));
    }
    
    var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken).ConfigureAwait(false) ?? "";
    var splitCodes = mergedCodes.Split(';');
    if (splitCodes.Contains(code))
    {
      var updatedCodes = new List<string>(splitCodes.Where(s => s != code));
      await ReplaceCodesAsync(user, updatedCodes, cancellationToken).ConfigureAwait(false);
      return true;
    }
    return false;
  }

  
  /// <inheritdoc />
  public async Task<int> CountCodesAsync(TUser user, CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();

    if (user == null)
    {
      throw new ArgumentNullException(nameof(user));
    }
    var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken).ConfigureAwait(false) ?? "";
    if (mergedCodes.Length > 0)
    {
      return mergedCodes.Split(';').Length;
    }
    return 0;
  }


  /// <inheritdoc />
  public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
  {
    user.PhoneNumber = phoneNumber;
    return Task.CompletedTask;
  }


  /// <inheritdoc />
  public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken) =>
    Task.FromResult(user.PhoneNumber);


  /// <inheritdoc />
  public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken) =>
    Task.FromResult(user.IsPhoneNumberConfirmed);


  /// <inheritdoc />
  public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
  {
    user.IsPhoneNumberConfirmed = confirmed;
    return Task.CompletedTask;
  }
}
