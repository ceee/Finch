using Microsoft.AspNetCore.Identity;

namespace zero;

public class RavenCoreRoleStore<TRole> : RavenRoleStore<TRole> 
  where TRole : ZeroIdentityRole
{
  public RavenCoreRoleStore(IZeroStore store, IZeroOptions options, IdentityErrorDescriber describer = null) : base(store, options, describer, true) { }
}



public class RavenCoreUserStore<TUser> : RavenUserStore<TUser>
  where TUser : ZeroIdentityUser
{
  public RavenCoreUserStore(IZeroStore store, IZeroOptions options) : base(store, options, true) { }
}



public class RavenCoreUserStore<TUser, TRole> : RavenUserStore<TUser, TRole>
  where TUser : ZeroIdentityUser
  where TRole : ZeroIdentityRole
{
  public RavenCoreUserStore(IZeroStore store, IZeroOptions options) : base(store, options, true) { }

}