using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public partial class RoleStore : IRoleClaimStore<IUserRole>
  {
    /// <inheritdoc/>
    public async Task AddClaimAsync(IUserRole role, Claim claim, CancellationToken cancellationToken = default)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        role.Claims.Add(new UserClaim(claim));
        await session.StoreAsync(role, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }


    /// <inheritdoc/>
    public Task<IList<Claim>> GetClaimsAsync(IUserRole role, CancellationToken cancellationToken = default)
    {
      return Task.FromResult((IList<Claim>)role.Claims.Select(claim => claim.ToClaim()).ToList());
    }


    /// <inheritdoc/>
    public async Task RemoveClaimAsync(IUserRole role, Claim claim, CancellationToken cancellationToken = default)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        IUserClaim userClaim = new UserClaim(claim);

        role.Claims = role.Claims.Except(new List<IUserClaim>() { userClaim }, new UserClaimComparer()).ToList();

        await session.StoreAsync(role, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
