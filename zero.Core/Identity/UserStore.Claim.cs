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
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public partial class UserStore<TUser> : IUserClaimStore<TUser> where TUser : class, IIdentityUser
  {
    /// <inheritdoc />
    public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        user.Claims.AddRange(claims.Select(claim => new UserClaim(claim)));
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }


    /// <inheritdoc />
    public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult((IList<Claim>)user.Claims.Select(claim => claim.ToClaim()).ToList());
    }


    /// <inheritdoc />
    public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        UserClaim userClaim = new UserClaim(claim);
        return await session.Query<TUser>().Where(x => x.Claims.Any(c => c.Type == userClaim.Type && c.Value == userClaim.Value)).ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        IEnumerable<UserClaim> userClaims = claims.Select(c => new UserClaim(c)).ToList();

        user.Claims = user.Claims.Except(userClaims, new UserClaimComparer()).ToList();

        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }


    /// <inheritdoc />
    public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        UserClaim userClaim = new UserClaim(claim);
        UserClaim newUserClaim = new UserClaim(newClaim);

        user.Claims.Remove(userClaim);
        user.Claims.Add(newUserClaim);

        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
