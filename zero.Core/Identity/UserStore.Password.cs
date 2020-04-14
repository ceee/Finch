using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents.Session;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Identity
{
  public partial class UserStore<TUser> : IUserPasswordStore<TUser> where TUser : class, IUser
  {
    /// <inheritdoc />
    public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.PasswordHash);
    }


    /// <inheritdoc />
    public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(!user.PasswordHash.IsNullOrEmpty());
    }


    /// <inheritdoc />
    public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
    {
      user.PasswordHash = passwordHash;
      return Task.CompletedTask;
    }
  }
}
