using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents.Session;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Identity
{
  public partial class UserStore : IUserPasswordStore<IUser>
  {
    /// <inheritdoc />
    public Task<string> GetPasswordHashAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.PasswordHash);
    }


    /// <inheritdoc />
    public Task<bool> HasPasswordAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(!user.PasswordHash.IsNullOrEmpty());
    }


    /// <inheritdoc />
    public async Task SetPasswordHashAsync(IUser user, string passwordHash, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        user.PasswordHash = passwordHash;
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
