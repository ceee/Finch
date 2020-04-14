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
  public partial class UserStore<TUser> : IUserRoleStore<TUser> where TUser : class, IUser
  {
    /// <inheritdoc />
    public Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
    {  
      user.RoleIds.Add(roleName);
      return Task.CompletedTask;
    }


    /// <inheritdoc />
    public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult((IList<string>)user.RoleIds.ToList());
    }


    /// <inheritdoc />
    public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<TUser>().Where(x => roleName.In(x.RoleIds)).ToListAsync();
      }
    }


    /// <inheritdoc />
    public Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.RoleIds.Contains(roleName, StringComparer.InvariantCultureIgnoreCase));
    }


    /// <inheritdoc />
    public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
    {
      user.RoleIds.Remove(roleName);
      return Task.CompletedTask;
    }
  }
}
