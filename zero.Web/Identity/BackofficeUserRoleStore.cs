using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Web.Identity
{
  /// <summary>
  /// Provides an abstraction for a store of role specific claims.
  /// </summary>
  public class BackofficeUserRoleStore<TRole, TClaim> : IRoleClaimStore<TRole> 
    where TClaim : class, IBackofficeUserClaim, new()
    where TRole : class, IBackofficeUserRole, new()
  {
    /// <summary>
    /// Gets the database context for this store.
    /// </summary>
    IDocumentStore Raven { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="IdentityErrorDescriber"/> for any error that occurred with the current operation.
    /// </summary>
    public IdentityErrorDescriber ErrorDescriber { get; set; }


    public BackofficeUserRoleStore(IDocumentStore raven, IdentityErrorDescriber describer = null)
    {
      Raven = raven ?? throw new ArgumentNullException(nameof(raven));
      ErrorDescriber = describer ?? new IdentityErrorDescriber();
    }


    /// <inheritdoc/>
    async Task SaveChangesAsync(TRole role, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        await session.StoreAsync(role, cancellationToken);
      }
    }


    /// <inheritdoc/>
    public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
    {
      await SaveChangesAsync(role, cancellationToken);
      return IdentityResult.Success;
    }


    /// <inheritdoc/>
    public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        try
        {
          session.Delete(role.Id);
          await session.SaveChangesAsync(cancellationToken);
        }
        catch (ConcurrencyException)
        {
          return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }
        return IdentityResult.Success;
      }
    }


    /// <inheritdoc/>
    public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return session.LoadAsync<TRole>(roleId, cancellationToken);
      }
    }


    /// <inheritdoc/>
    public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return session.Query<TRole>().FirstOrDefaultAsync(x => x.Name == normalizedRoleName);
      }
    }


    /// <inheritdoc/>
    public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
    {
      return Task.FromResult(role.Name.ToLowerInvariant());
    }


    /// <inheritdoc/>
    public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
    {
      return Task.FromResult(role.Id);
    }


    /// <inheritdoc/>
    public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
    {
      return Task.FromResult(role.Name);
    }


    /// <inheritdoc/>
    public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
    {
      return Task.FromResult(0);
    }


    /// <inheritdoc/>
    public async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
    {
      role.Name = roleName;
      await SaveChangesAsync(role, cancellationToken);
    }


    /// <inheritdoc/>
    public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
    {
      try
      {
        await SaveChangesAsync(role, cancellationToken);
      }
      catch (ConcurrencyException)
      {
        return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
      }
      return IdentityResult.Success;
    }


    /// <inheritdoc/>
    public async Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
    {
      role.Claims.Add(new TClaim().FromClaim(claim));
      await SaveChangesAsync(role, cancellationToken);
    }


    /// <inheritdoc/>
    public Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
    {
      return Task.FromResult<IList<Claim>>(role.Claims.Select(c => c.ToClaim()).ToList());
    }


    /// <inheritdoc/>
    public async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
    {
      role.Claims = role.Claims.Where(c => c.Type == claim.Type).ToList();
      await SaveChangesAsync(role, cancellationToken);
    }


    /// <inheritdoc/>
    public void Dispose() { }
  }
}
