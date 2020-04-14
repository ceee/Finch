using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public partial class UserStore : IUserRoleStore<IUser>
  {
    /// <inheritdoc />
    public async Task AddToRoleAsync(IUser user, string roleName, CancellationToken cancellationToken)
    {  
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        user.RoleIds.Add(roleName);
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }


    /// <inheritdoc />
    public Task<IList<string>> GetRolesAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult((IList<string>)user.RoleIds.ToList());
    }


    /// <inheritdoc />
    public async Task<IList<IUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<IUser>().Where(x => roleName.In(x.RoleIds)).ToListAsync();
      }
    }


    /// <inheritdoc />
    public Task<bool> IsInRoleAsync(IUser user, string roleName, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.RoleIds.Contains(roleName, StringComparer.InvariantCultureIgnoreCase));
    }


    /// <inheritdoc />
    public async Task RemoveFromRoleAsync(IUser user, string roleName, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        user.RoleIds.Remove(roleName);
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
