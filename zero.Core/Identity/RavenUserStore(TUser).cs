using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Identity
{
  public partial class RavenUserStore<TUser> : 
    IUserStore<TUser>,
    IUserEmailStore<TUser>,
    IUserLockoutStore<TUser>,
    IUserPasswordStore<TUser>,
    IUserClaimStore<TUser>,
    IUserSecurityStampStore<TUser>,
    IProtectedUserStore<TUser>
    where TUser : class, IIdentityUser
  {
    protected IZeroStore Store { get; private set; }

    public RavenUserStore(IZeroStore store)
    {
      Store = store;
    }


    /// <summary>
    /// Get the Raven compare/exchange key for this user.
    /// This should be unique within the store, otherwise you can't create users with the same email in different stores.
    /// </summary>
    protected virtual string GetReservationKey(TUser user) => Constants.Database.ReservationPrefix + typeof(TUser) + ':' + user.Email;

    // <summary>
    /// Scope queries to an optional application or something else
    /// </summary>
    protected virtual IRavenQueryable<TUser> ScopeQuery(IRavenQueryable<TUser> query) => query;


    // <summary>
    /// Determines whether a passed user is part of this store and should be processed
    /// </summary>
    protected virtual bool IsUserPartOfStore(TUser role) => true;


    /// <inheritdoc />
    public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
    {
      if (!IsUserPartOfStore(user))
      {
        return IdentityResult.Failed(new IdentityError()
        {
          Code = "NotPartOfStore",
          Description = $"The affected user is is not part of this user store and can't be created."
        });
      }

      using IAsyncDocumentSession session = Store.OpenAsyncSession();

      // try to reserve the key for the new user
      if (!await Store.RavenStore.ReserveAsync(GetReservationKey(user), user.Id))
      {
        return IdentityResult.Failed(new IdentityError
        {
          Code = "DuplicateEmail",
          Description = $"The email address {user.Email} is already taken."
        });
      }

      await session.StoreAsync(user, cancellationToken);

      // Because this a a cluster-wide operation due to compare/exchange tokens,
      // we need to save changes here; if we can't store the user, 
      // we need to roll back the email reservation.
      try
      {
        await session.SaveChangesAsync(cancellationToken);
      }
      catch (Exception)
      {
        // The compare/exchange email reservation is cluster-wide, outside of the session scope. 
        // We need to manually roll it back.
        await Store.RavenStore.RemoveReservationAsync(GetReservationKey(user));
        throw;
      }

      return IdentityResult.Success;
    }


    /// <inheritdoc />
    public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
    {
      if (!IsUserPartOfStore(user))
      {
        return IdentityResult.Failed(new IdentityError()
        {
          Code = "NotPartOfStore",
          Description = $"The affected user is is not part of this user store and can't be deleted."
        });
      }

      // Remove the cluster-wide compare/exchange key.
      await Store.RavenStore.RemoveReservationAsync(GetReservationKey(user));

      // Delete the user and save it. We must save it because deleting is a cluster-wide operation.
      // Only if the deletion succeeds will we remove the cluster-wide compare/exchange key.
      using IAsyncDocumentSession session = Store.OpenAsyncSession();

      TUser source = await session.LoadAsync<TUser>(user.Id, cancellationToken);

      session.Delete(source);
      await session.SaveChangesAsync(cancellationToken);

      return IdentityResult.Success;
    }


    /// <inheritdoc />
    public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
    {
      if (!IsUserPartOfStore(user))
      {
        return IdentityResult.Failed(new IdentityError()
        {
          Code = "NotPartOfStore",
          Description = $"The affected user is is not part of this user store and can't be updated."
        });
      }

      using (IAsyncDocumentSession session = Store.OpenAsyncSession())
      {
        TUser source = await session.LoadAsync<TUser>(user.Id, cancellationToken);

        if (source == null)
        {
          return IdentityResult.Failed(new IdentityError
          {
            Code = "UserNotFound",
            Description = $"Could not find stored user with id {user.Id}."
          });
        }

        if (source.Email != user.Email)
        {
          // try to reserve the key for the new user
          if (!await Store.RavenStore.ReserveAsync(GetReservationKey(user), user.Id))
          {
            return IdentityResult.Failed(new IdentityError
            {
              Code = "DuplicateEmail",
              Description = $"The email address {user.Email} is already taken."
            });
          }

          // Remove the cluster-wide compare/exchange key.
          await Store.RavenStore.RemoveReservationAsync(GetReservationKey(source));
        }
      }

      using (IAsyncDocumentSession session = Store.OpenAsyncSession())
      {
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }

      return IdentityResult.Success;
    }


    /// <inheritdoc />
    public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return await ScopeQuery(session.Query<TUser>()).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }


    /// <inheritdoc />
    public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return await ScopeQuery(session.Query<TUser>()).FirstOrDefaultAsync(x => x.Username == normalizedUserName, cancellationToken);
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
    public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
    {
      await SetNormalizedUserNameAsync(user, userName, cancellationToken);
    }


    /// <inheritdoc />
    void IDisposable.Dispose() { }


    /*
     * ****************************************************
     * SECURITY STAMP
     * ****************************************************
     */


    /// <inheritdoc />
    public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.SecurityStamp);
    }


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
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return await ScopeQuery(session.Query<TUser>()).FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);
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
    public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
      if (!IsUserPartOfStore(user))
      {
        // TODO we should log this as we can't return an IdentityResult
        return;
      }

      using IAsyncDocumentSession session = Store.OpenAsyncSession();

      user.Claims.AddRange(claims.Select(claim => new UserClaim(claim)));
      await session.StoreAsync(user, cancellationToken);
      await session.SaveChangesAsync(cancellationToken);
    }


    /// <inheritdoc />
    public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult((IList<Claim>)user.Claims.Select(claim => claim.ToClaim()).ToList());
    }


    /// <inheritdoc />
    public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      UserClaim userClaim = new UserClaim(claim);
      return await ScopeQuery(session.Query<TUser>()).Where(x => x.Claims.Any(c => c.Type == userClaim.Type && c.Value == userClaim.Value)).ToListAsync();
    }


    /// <inheritdoc />
    public async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
      if (!IsUserPartOfStore(user))
      {
        // TODO we should log this as we can't return an IdentityResult
        return;
      }

      using IAsyncDocumentSession session = Store.OpenAsyncSession();

      IEnumerable<UserClaim> userClaims = claims.Select(c => new UserClaim(c)).ToList();

      user.Claims = user.Claims.Except(userClaims, new UserClaimComparer()).ToList();

      await session.StoreAsync(user, cancellationToken);
      await session.SaveChangesAsync(cancellationToken);

    }


    /// <inheritdoc />
    public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
      if (!IsUserPartOfStore(user))
      {
        // TODO we should log this as we can't return an IdentityResult
        return;
      }

      using IAsyncDocumentSession session = Store.OpenAsyncSession();

      UserClaim userClaim = new UserClaim(claim);
      UserClaim newUserClaim = new UserClaim(newClaim);

      user.Claims.Remove(userClaim);
      user.Claims.Add(newUserClaim);

      await session.StoreAsync(user, cancellationToken);
      await session.SaveChangesAsync(cancellationToken);
    }
  }
}
