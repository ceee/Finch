using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace zero.Identity;

public static class IdentityBuilderExtensions
{
  /// <summary>
  /// Adds an implementation of identity information stores.
  /// </summary>
  public static IdentityBuilder AddZeroIdentityStores<T>(this IdentityBuilder builder) where T : class, IZeroIdentityStoreDbProvider
  {
    Type userStoreType = typeof(ZeroUserStore<,>).MakeGenericType(builder.UserType, builder.RoleType);
    Type roleStoreType = typeof(ZeroRoleStore<>).MakeGenericType(builder.RoleType);

    builder.Services.AddScoped<IZeroIdentityStoreDbProvider, T>();
    builder.Services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(builder.UserType), userStoreType);
    builder.Services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(builder.RoleType), roleStoreType);
    
    return builder;
  }
}