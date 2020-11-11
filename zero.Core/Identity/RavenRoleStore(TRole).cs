using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Client.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public class RavenRoleStore<TRole> : IRoleStore<TRole>, IRoleClaimStore<TRole> 
    where TRole : class, IIdentityUserRole
  {
    protected IDocumentStore Raven { get; private set; }

    protected IdentityErrorDescriber ErrorDescriber { get; private set; }


    public RavenRoleStore(IDocumentStore raven, IdentityErrorDescriber describer = null)
    {
      Raven = raven;
      ErrorDescriber = describer ?? new IdentityErrorDescriber();
    }


    // <summary>
    /// Scope queries to an optional application or something else
    /// </summary>
    protected virtual IRavenQueryable<TRole> ScopeQuery(IRavenQueryable<TRole> query) => query;


    // <summary>
    /// Determines whether a passed user role is part of this store and should be processed
    /// </summary>
    protected virtual bool IsRolePartOfStore(TRole role) => true;


    /// <inheritdoc/>
    public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
    {
      if (!IsRolePartOfStore(role))
      {
        return IdentityResult.Failed(new IdentityError()
        {
          Code = "NotPartOfStore",
          Description = $"The affected role is is not part of this role store and can't be created."
        });
      }
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        await session.StoreAsync(role);
        await session.SaveChangesAsync(cancellationToken);
      }
      return IdentityResult.Success;
    }


    /// <inheritdoc/>
    public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
    {
      if (!IsRolePartOfStore(role))
      {
        return IdentityResult.Failed(new IdentityError()
        {
          Code = "NotPartOfStore",
          Description = $"The affected role is is not part of this role store and can't be updatd."
        });
      }
      try
      {
        using IAsyncDocumentSession session = Raven.OpenAsyncSession();
        await session.StoreAsync(role, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
      catch (ConcurrencyException)
      {
        return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
      }
      return IdentityResult.Success;
    }


    /// <inheritdoc/>
    public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
    {
      if (!IsRolePartOfStore(role))
      {
        return IdentityResult.Failed(new IdentityError()
        {
          Code = "NotPartOfStore",
          Description = $"The affected role is is not part of this role store and can't be deleted."
        });
      }
      try
      {
        using IAsyncDocumentSession session = Raven.OpenAsyncSession();
        session.Delete(role);
        await session.SaveChangesAsync(cancellationToken);
      }
      catch (ConcurrencyException)
      {
        return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
      }
      return IdentityResult.Success;
    }


    /// <inheritdoc/>
    public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken) => Task.FromResult(role.Id);


    /// <inheritdoc/>
    public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken) => Task.FromResult(role.Name);


    /// <inheritdoc/>
    public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
    {
      role.Name = roleName;
      return Task.CompletedTask;
    }


    /// <inheritdoc/>
    public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken) => Task.FromResult(role.Name);


    /// <inheritdoc/>
    public async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
    {
      await SetRoleNameAsync(role, normalizedName, cancellationToken);
    }


    /// <inheritdoc/>
    public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      return await ScopeQuery(session.Query<TRole>()).FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
    }


    /// <inheritdoc/>
    public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      return await ScopeQuery(session.Query<TRole>()).FirstOrDefaultAsync(x => x.Name == normalizedRoleName, cancellationToken);
    }


    /// <inheritdoc/>
    public void Dispose()
    {
      
    }


    /*
     * ****************************************************
     * CLAIM
     * ****************************************************
     */


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
