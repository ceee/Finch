using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents.Session;
using System;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public partial class UserStore : IUserLockoutStore<IUser>
  {
    /// <inheritdoc />
    public Task<int> GetAccessFailedCountAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.AccessFailedCount);
    }


    /// <inheritdoc />
    public Task<bool> GetLockoutEnabledAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.LockoutEnabled);
    }


    /// <inheritdoc />
    public Task<DateTimeOffset?> GetLockoutEndDateAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.LockoutEnd);
    }


    /// <inheritdoc />
    public async Task<int> IncrementAccessFailedCountAsync(IUser user, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        user.AccessFailedCount += 1;
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }

      return user.AccessFailedCount;
    }


    /// <inheritdoc />
    public async Task ResetAccessFailedCountAsync(IUser user, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        user.AccessFailedCount = 0;
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }


    /// <inheritdoc />
    public async Task SetLockoutEnabledAsync(IUser user, bool enabled, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        user.LockoutEnabled = enabled;
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }


    /// <inheritdoc />
    public async Task SetLockoutEndDateAsync(IUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        user.LockoutEnd = lockoutEnd;
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
