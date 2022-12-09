using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using zero.Identity;
using zero.Raven;

namespace zero.Raven;

public class RavenRoleStore<TRole> :
  IRoleStore<TRole>, 
  IRoleClaimStore<TRole>
  where TRole : ZeroIdentityRole, new()
{
  protected IdentityErrorDescriber ErrorDescriber { get; private set; }

  protected virtual IRavenOperations Ops { get; set; }


  public RavenRoleStore(IRavenOperations operations, IdentityErrorDescriber describer = null)
  {
    Ops = operations;
    ErrorDescriber = describer ?? new IdentityErrorDescriber();
  }


  private IdentityResult Fail(Result<TRole> result)
  {
    IdentityError[] errors = new IdentityError[result.Errors.Count];

    int index = 0;
    foreach (ResultError error in result.Errors)
    {
      string message = error.Message + "(key: " + error.Property + ")";
      errors[index++] = new() { Code = "zero/raven/500", Description = message };
    }

    return IdentityResult.Failed(errors);
  }


  /// <inheritdoc/>
  public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();

    if (role == null)
    {
      throw new ArgumentNullException(nameof(role));
    }

    try
    {
      Result<TRole> result =  await Ops.Create(role);

      if (!result.IsSuccess)
      {
        return Fail(result);
      }
    }
    catch (Exception ex)
    {
      return IdentityResult.Failed(new IdentityError { Description = ex.Message });
    }

    return IdentityResult.Success;
  }


  /// <inheritdoc/>
  public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
  {
    try
    {
      Result<TRole> result = await Ops.Update(role);
      
      if (!result.IsSuccess)
      {
        return Fail(result);
      }
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
      Result<TRole> result = await Ops.Delete(role);
      
      if (!result.IsSuccess)
      {
        return Fail(result);
      }
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
  public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken) 
    => Task.FromResult(role.Name);


  /// <inheritdoc/>
  public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken) =>
    SetRoleNameAsync(role, normalizedName, cancellationToken);


  /// <inheritdoc/>
  public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
  {
    // TODO index
    return await Ops.Session.Query<TRole>().FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
  }


  /// <inheritdoc/>
  public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
  {
    // TODO index
    return await Ops.Session.Query<TRole>().FirstOrDefaultAsync(x => x.Name == normalizedRoleName, cancellationToken);
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
  public Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default) =>
    Task.FromResult((IList<Claim>)role.Claims.Select(claim => claim.ToClaim()).ToList());


  /// <inheritdoc/>
  public Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
  {
    UserClaim userClaim = new(claim);
    role.Claims = role.Claims.Except(new List<UserClaim>() { userClaim }, new UserClaimComparer()).ToList();
    return Task.CompletedTask;
  }
}