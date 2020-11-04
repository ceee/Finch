using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents.Session;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public partial class UserStore<TUser> : IUserSecurityStampStore<TUser> where TUser : class, IIdentityUser
  {
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
  }
}
