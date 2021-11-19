using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Options;

namespace zero.Core.Identity
{
  // TODO we can't inject IZeroContext here for app-context as the IApplicationContext itself
  // relies on UserManager and therefore this UserStore, i.e. circular dependency

  public partial class RavenUserStore<TUser, TRole> : RavenUserStore<TUser>,
    IUserRoleStore<TUser>
    where TUser : ZeroIdentityUser
    where TRole : ZeroIdentityRole
  {
    public RavenUserStore(IZeroStore store, IZeroOptions options, bool global = false) : base(store, options, global) { }


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
      return await ScopeQuery(Store.Session(Global).Query<TUser>()).Where(x => roleName.In(x.RoleIds)).ToListAsync();
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
