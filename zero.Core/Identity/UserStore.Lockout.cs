using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents.Session;
using System;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public partial class RavenUserStore<TUser> : IUserLockoutStore<TUser> where TUser : class, IIdentityUser
  {
    /// <inheritdoc />
    public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.AccessFailedCount);
    }


    /// <inheritdoc />
    public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.LockoutEnabled);
    }


    /// <inheritdoc />
    public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.LockoutEnd);
    }


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
  }
}
