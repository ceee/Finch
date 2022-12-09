using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace zero.Raven;

public static class IdentityBuilderExtensions
{
  /// <summary>
  /// Adds a RavenDb implementation of identity information stores.
  /// </summary>
  public static IdentityBuilder AddRavenDbStores(this IdentityBuilder builder)
  {
    Type userStoreType = typeof(RavenUserStore<,>).MakeGenericType(builder.UserType, builder.RoleType);
    Type roleStoreType = typeof(RavenRoleStore<>).MakeGenericType(builder.RoleType);
    
    builder.Services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(builder.UserType), userStoreType);
    builder.Services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(builder.RoleType), roleStoreType);
    
    return builder;
  }
}