using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Identity
{
  public partial class RoleStore : IRoleStore<IUserRole>
  {
    protected IDocumentStore Raven { get; private set; }

    protected IdentityErrorDescriber ErrorDescriber { get; private set; }


    public RoleStore(IDocumentStore raven, IdentityErrorDescriber describer = null)
    {
      Raven = raven;
      ErrorDescriber = describer ?? new IdentityErrorDescriber();
    }


    /// <inheritdoc/>
    public async Task<IdentityResult> CreateAsync(IUserRole role, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        await session.StoreAsync(role);
        await session.SaveChangesAsync(cancellationToken);
      }
      return IdentityResult.Success;
    }


    /// <inheritdoc/>
    public async Task<IdentityResult> UpdateAsync(IUserRole role, CancellationToken cancellationToken)
    {
      try
      {
        using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
        {
          await session.StoreAsync(role, cancellationToken);
          await session.SaveChangesAsync(cancellationToken);
        }
      }
      catch (ConcurrencyException)
      {
        return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
      }
      return IdentityResult.Success;
    }


    /// <inheritdoc/>
    public async Task<IdentityResult> DeleteAsync(IUserRole role, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        try
        {
          session.Delete(role);
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
    public Task<string> GetRoleIdAsync(IUserRole role, CancellationToken cancellationToken)
    {
      return Task.FromResult(role.Id);
    }


    /// <inheritdoc/>
    public Task<string> GetRoleNameAsync(IUserRole role, CancellationToken cancellationToken)
    {
      return Task.FromResult(role.Name);
    }


    /// <inheritdoc/>
    public async Task SetRoleNameAsync(IUserRole role, string roleName, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        role.Name = roleName;
        await session.StoreAsync(role, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }


    /// <inheritdoc/>
    public Task<string> GetNormalizedRoleNameAsync(IUserRole role, CancellationToken cancellationToken)
    {
      return Task.FromResult(role.Name);
    }


    /// <inheritdoc/>
    public async Task SetNormalizedRoleNameAsync(IUserRole role, string normalizedName, CancellationToken cancellationToken)
    {
      await SetRoleNameAsync(role, normalizedName, cancellationToken);
    }


    /// <inheritdoc/>
    public async Task<IUserRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.LoadAsync<IUserRole>(roleId);
      }
    }


    /// <inheritdoc/>
    public async Task<IUserRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<IUserRole>().FirstOrDefaultAsync(x => x.Name == normalizedRoleName, cancellationToken);
      }
    }


    /// <inheritdoc/>
    public void Dispose()
    {
      
    }
  }
}
