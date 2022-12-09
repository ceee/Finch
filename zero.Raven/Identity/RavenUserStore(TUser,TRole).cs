using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using zero.Raven;

namespace zero.Identity;

public partial class RavenUserStore<TUser, TRole> : RavenUserStore<TUser>,
  IUserRoleStore<TUser>
  where TUser : ZeroIdentityUser, new()
  where TRole : ZeroIdentityRole, new()
{
  public RavenUserStore(IRavenOperations operations) : base(operations) { }


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
    return await Ops.Session.Query<TUser>().Where(x => roleName.In(x.RoleIds)).ToListAsync();
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
