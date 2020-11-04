using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public partial class RoleStore<TRole> : IRoleStore<TRole> where TRole : class, IIdentityUserRole
  {
    protected IDocumentStore Raven { get; private set; }

    protected IdentityErrorDescriber ErrorDescriber { get; private set; }


    public RoleStore(IDocumentStore raven, IdentityErrorDescriber describer = null)
    {
      Raven = raven;
      ErrorDescriber = describer ?? new IdentityErrorDescriber();
    }


    /// <inheritdoc/>
    public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
    {
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
    public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
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
    public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
    {
      role.Name = roleName;
      return Task.CompletedTask;
    }


    /// <inheritdoc/>
    public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
    {
      return Task.FromResult(role.Name);
    }


    /// <inheritdoc/>
    public async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
    {
      await SetRoleNameAsync(role, normalizedName, cancellationToken);
    }


    /// <inheritdoc/>
    public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.LoadAsync<TRole>(roleId);
      }
    }


    /// <inheritdoc/>
    public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<TRole>().FirstOrDefaultAsync(x => x.Name == normalizedRoleName, cancellationToken);
      }
    }


    /// <inheritdoc/>
    public void Dispose()
    {
      
    }
  }
}
