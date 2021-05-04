using Microsoft.AspNetCore.Identity;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Options;

namespace zero.Core.Identity
{
  public class RavenCoreRoleStore<TRole> : RavenRoleStore<TRole> 
    where TRole : ZeroIdentityRole
  {
    public RavenCoreRoleStore(IZeroStore store, IZeroOptions options, IdentityErrorDescriber describer = null) : base(store, options, describer) { }

    /// <inheritdoc/>
    protected override string GetDatabase() => Options.Raven.Database;
  }



  public class RavenCoreUserStore<TUser> : RavenUserStore<TUser>
    where TUser : ZeroIdentityUser
  {
    public RavenCoreUserStore(IZeroStore store, IZeroOptions options) : base(store, options) { }

    /// <inheritdoc/>
    protected override string GetDatabase() => Options.Raven.Database;
  }



  public class RavenCoreUserStore<TUser, TRole> : RavenUserStore<TUser, TRole>
    where TUser : ZeroIdentityUser
    where TRole : ZeroIdentityRole
  {
    public RavenCoreUserStore(IZeroStore store, IZeroOptions options) : base(store, options) { }

    /// <inheritdoc/>
    protected override string GetDatabase() => Options.Raven.Database;
  }
}
