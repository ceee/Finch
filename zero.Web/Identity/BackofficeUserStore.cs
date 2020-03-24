using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Web.Identity
{
  /// <summary>
  /// UserStore for entities in a RavenDB database.
  /// </summary>
  public class BackofficeUserStore<TUser> :
    IUserStore<TUser>,
    IUserClaimStore<TUser>,
    IUserRoleStore<TUser>,
    IUserLockoutStore<TUser>,
    IUserSecurityStampStore<TUser>,
    IUserPasswordStore<TUser>,
    IQueryableUserStore<TUser>,
    IUserTwoFactorStore<TUser>,
    IUserTwoFactorRecoveryCodeStore<TUser>,
    IDisposable
    where TUser : class, IBackofficeUser, new()
  {
    /// <summary>
    /// Gets the database context for this store.
    /// </summary>
    IDocumentStore Raven { get; set; }


    /// <inheritdoc/>
    public IQueryable<TUser> Users => throw new NotImplementedException();


    public BackofficeUserStore(IDocumentStore raven)
    {
      Raven = raven ?? throw new ArgumentNullException(nameof(raven));
    }


    /// <inheritdoc/>
    public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public void Dispose()
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<int> CountCodesAsync(TUser user, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task<bool> RedeemCodeAsync(TUser user, string code, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc/>
    public Task ReplaceCodesAsync(TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
