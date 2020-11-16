using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Identity
{
  // TODO we can't inject IZeroContext here for app-context as the IApplicationContext itself
  // relies on UserManager and therefore this UserStore, i.e. circular dependency

  public partial class RavenUserStore<TUser, TRole> : RavenUserStore<TUser>,
    IUserRoleStore<TUser>
    where TUser : class, IIdentityUserWithRoles
    where TRole : class, IIdentityUserRole
  {
    public RavenUserStore(IZeroStore store, IZeroOptions options) : base(store, options) { }


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
      using IAsyncDocumentSession session = Store.OpenCoreSession();
      return await ScopeQuery(session.Query<TUser>()).Where(x => roleName.In(x.RoleIds)).ToListAsync(); // TODO scope     
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
