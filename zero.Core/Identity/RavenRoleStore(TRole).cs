using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Exceptions;
using System.Security.Claims;

namespace zero.Identity;

public class RavenRoleStore<TRole> : 
  EntityStore<TRole>,
  IRoleStore<TRole>, 
  IRoleClaimStore<TRole> 
  where TRole : ZeroIdentityRole, new()
{
  protected IdentityErrorDescriber ErrorDescriber { get; private set; }

  protected bool Global { get; private set; }


  public RavenRoleStore(IStoreContext storeContext, IdentityErrorDescriber describer = null, bool global = false) : base(storeContext)
  {
    Global = global;
    Config.Database = global ? Options.For<RavenOptions>().Database : null;
    Config.IncludeInactive = true;
    ErrorDescriber = describer ?? new IdentityErrorDescriber();
  }


  // <summary>
  /// Scope queries to an optional application or something else
  /// </summary>
  protected virtual IRavenQueryable<TRole> ScopeQuery(IRavenQueryable<TRole> query) => query;


  /// <inheritdoc/>
  public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
  {
    await Create(role);
    return IdentityResult.Success;
  }


  /// <inheritdoc/>
  public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
  {
    try
    {
      await Update(role);
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
    try
    {
      await Delete(role);
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
    return await ScopeQuery(Session.Query<TRole>()).FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
  }


  /// <inheritdoc/>
  public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
  {
    return await ScopeQuery(Session.Query<TRole>()).FirstOrDefaultAsync(x => x.Name == normalizedRoleName, cancellationToken);
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
    role.Claims.Add(new(claim));
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
    UserClaim userClaim = new(claim);
    role.Claims = role.Claims.Except(new List<UserClaim>() { userClaim }, new UserClaimComparer()).ToList();
    return Task.CompletedTask;
  }
}