using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public partial class UserStore<TUser> : IUserEmailStore<TUser> where TUser : class, IUser
  {
    /// <inheritdoc />
    public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<TUser>().FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);
      }
    }


    /// <inheritdoc />
    public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.Email);
    }


    /// <inheritdoc />
    public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.IsEmailConfirmed);
    }


    /// <inheritdoc />
    public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.Email);
    }


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
  }
}
