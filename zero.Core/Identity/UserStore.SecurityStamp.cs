using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents.Session;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public partial class UserStore : IUserSecurityStampStore<IUser>
  {
    /// <inheritdoc />
    public Task<string> GetSecurityStampAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.SecurityStamp);
    }


    /// <inheritdoc />
    public async Task SetSecurityStampAsync(IUser user, string stamp, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        user.SecurityStamp = stamp;
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
