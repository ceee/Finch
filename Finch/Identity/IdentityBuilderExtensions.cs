using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Finch.Identity;

public static class IdentityBuilderExtensions
{
  /// <summary>
  /// Adds an implementation of identity information stores.
  /// </summary>
  public static IdentityBuilder AddFinchIdentityStores(this IdentityBuilder builder)
  {
    Type userStoreType = typeof(FinchUserStore<,>).MakeGenericType(builder.UserType, builder.RoleType);
    Type roleStoreType = typeof(FinchRoleStore<>).MakeGenericType(builder.RoleType);
    builder.Services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(builder.UserType), userStoreType);
    builder.Services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(builder.RoleType), roleStoreType);
    
    return builder;
  }
}