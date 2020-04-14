using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public partial class UserStore : IUserEmailStore<IUser>
  {
    /// <inheritdoc />
    public async Task<IUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<IUser>().FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);
      }
    }


    /// <inheritdoc />
    public Task<string> GetEmailAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.Email);
    }


    /// <inheritdoc />
    public Task<bool> GetEmailConfirmedAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.IsEmailConfirmed);
    }


    /// <inheritdoc />
    public Task<string> GetNormalizedEmailAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.Email);
    }


    /// <inheritdoc />
    public async Task SetEmailAsync(IUser user, string email, CancellationToken cancellationToken)
    {
      user.Email = email;
      await UpdateAsync(user, cancellationToken);
    }


    /// <inheritdoc />
    public async Task SetEmailConfirmedAsync(IUser user, bool confirmed, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        user.IsEmailConfirmed = confirmed;
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }


    /// <inheritdoc />
    public async Task SetNormalizedEmailAsync(IUser user, string normalizedEmail, CancellationToken cancellationToken)
    {
      user.Email = normalizedEmail;
      await UpdateAsync(user, cancellationToken);
    }
  }
}
