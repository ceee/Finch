using Microsoft.AspNetCore.Identity;

namespace zero.Identity;

public class RavenCoreRoleStore<TRole> : RavenRoleStore<TRole> 
  where TRole : ZeroIdentityRole, new()
{
  public RavenCoreRoleStore(IStoreContext context, IdentityErrorDescriber describer = null) : base(context, describer, true) { }
}



public class RavenCoreUserStore<TUser> : RavenUserStore<TUser>
  where TUser : ZeroIdentityUser, new()
{
  public RavenCoreUserStore(IStoreContext context) : base(context, true) { }
}



public class RavenCoreUserStore<TUser, TRole> : RavenUserStore<TUser, TRole>
  where TUser : ZeroIdentityUser, new()
  where TRole : ZeroIdentityRole, new()
{
  public RavenCoreUserStore(IStoreContext context) : base(context, true) { }

}