using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public partial class RoleStore<TRole> : IRoleClaimStore<TRole> where TRole : class, IIdentityUserRole
  {
    /// <inheritdoc/>
    public Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
    {
      role.Claims.Add(new UserClaim(claim));
      return Task.CompletedTask;
    }


    /// <inheritdoc/>
    public Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
    {
      return Task.FromResult((IList<Claim>)role.Claims.Select(claim => claim.ToClaim()).ToList());
    }


    /// <inheritdoc/>
    public Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
    {
      UserClaim userClaim = new UserClaim(claim);
      role.Claims = role.Claims.Except(new List<UserClaim>() { userClaim }, new UserClaimComparer()).ToList();
      return Task.CompletedTask;
    }
  }
}
