using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Identity
{
  public class RavenScopedRoleStore<TRole> : RavenRoleStore<TRole> 
    where TRole : class, IIdentityUserRole
  {
    protected IZeroContext Context { get; private set; }

    public RavenScopedRoleStore(IZeroStore store, IZeroContext context, IdentityErrorDescriber describer = null) : base(store, describer)
    {
      Context = context;
    }

    /// <inheritdoc/>
    protected override IRavenQueryable<TRole> ScopeQuery(IRavenQueryable<TRole> query) => query;

    /// <inheritdoc/>
    protected override bool IsRolePartOfStore(TRole role) => true;
  }



  public class RavenScopedUserStore<TUser> : RavenUserStore<TUser>
    where TUser : class, IIdentityUser
  {
    protected IZeroContext Context { get; private set; }

    public RavenScopedUserStore(IZeroStore store, IZeroContext context) : base(store)
    {
      Context = context;
    }

    /// <inheritdoc/>
    protected override string GetReservationKey(TUser user) => Constants.Database.ReservationPrefix + typeof(TUser) + ':' + user.Email;

    /// <inheritdoc/>
    protected override IRavenQueryable<TUser> ScopeQuery(IRavenQueryable<TUser> query) => query;

    /// <inheritdoc/>
    protected override bool IsUserPartOfStore(TUser user) => true;
  }



  public class RavenScopedUserStore<TUser, TRole> : RavenUserStore<TUser, TRole>
    where TUser : class, IIdentityUserWithRoles
    where TRole : class, IIdentityUserRole
  {
    protected IZeroContext Context { get; private set; }

    public RavenScopedUserStore(IZeroStore store, IZeroContext context) : base(store)
    {
      Context = context;
    }

    /// <inheritdoc/>
    protected override string GetReservationKey(TUser user) => Constants.Database.ReservationPrefix + typeof(TUser) + ':' + user.Email;

    /// <inheritdoc/>
    protected override IRavenQueryable<TUser> ScopeQuery(IRavenQueryable<TUser> query) => query;

    /// <inheritdoc/>
    protected override bool IsUserPartOfStore(TUser user) => true;
  }
}
