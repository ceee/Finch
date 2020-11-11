using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Identity
{
  public class RavenScopedRoleStore<TRole> : RavenRoleStore<TRole> 
    where TRole : class, IIdentityUserRole, IAppAwareEntity
  {
    protected IZeroContext Context { get; private set; }

    public RavenScopedRoleStore(IDocumentStore raven, IZeroContext context, IdentityErrorDescriber describer = null) : base(raven, describer)
    {
      Context = context;
    }

    /// <inheritdoc/>
    protected override IRavenQueryable<TRole> ScopeQuery(IRavenQueryable<TRole> query) => query.Scope(Context.AppId, false);

    /// <inheritdoc/>
    protected override bool IsRolePartOfStore(TRole role) => role.AppId.Equals(Context.AppId, StringComparison.InvariantCultureIgnoreCase);
  }



  public class RavenScopedUserStore<TUser> : RavenUserStore<TUser>
    where TUser : class, IIdentityUser, IAppAwareEntity
  {
    protected IZeroContext Context { get; private set; }

    public RavenScopedUserStore(IDocumentStore raven, IZeroContext context) : base(raven)
    {
      Context = context;
    }

    /// <inheritdoc/>
    protected override string GetReservationKey(TUser user) => Constants.Database.ReservationPrefix + typeof(TUser) + ':' + user.AppId + ':' + user.Email;

    /// <inheritdoc/>
    protected override IRavenQueryable<TUser> ScopeQuery(IRavenQueryable<TUser> query) => query.Scope(Context.AppId, false);

    /// <inheritdoc/>
    protected override bool IsUserPartOfStore(TUser user) => user.AppId.Equals(Context.AppId, StringComparison.InvariantCultureIgnoreCase);
  }



  public class RavenScopedUserStore<TUser, TRole> : RavenUserStore<TUser, TRole>
    where TUser : class, IIdentityUserWithRoles, IAppAwareEntity
    where TRole : class, IIdentityUserRole, IAppAwareEntity
  {
    protected IZeroContext Context { get; private set; }

    public RavenScopedUserStore(IDocumentStore raven, IZeroContext context) : base(raven)
    {
      Context = context;
    }

    /// <inheritdoc/>
    protected override string GetReservationKey(TUser user) => Constants.Database.ReservationPrefix + typeof(TUser) + ':' + user.AppId + ':' + user.Email;

    /// <inheritdoc/>
    protected override IRavenQueryable<TUser> ScopeQuery(IRavenQueryable<TUser> query) => query.Scope(Context.AppId, false);

    /// <inheritdoc/>
    protected override bool IsUserPartOfStore(TUser user) => user.AppId.Equals(Context.AppId, StringComparison.InvariantCultureIgnoreCase);
  }
}
