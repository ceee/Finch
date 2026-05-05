using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mixtape.Identity;

public static class IdentityBuilderExtensions
{
  /// <summary>
  /// Adds an implementation of identity information stores.
  /// </summary>
  public static IdentityBuilder AddMixtapeIdentityStores(this IdentityBuilder builder)
  {
    Type userStoreType = typeof(MixtapeUserStore<,>).MakeGenericType(builder.UserType, builder.RoleType);
    Type roleStoreType = typeof(MixtapeRoleStore<>).MakeGenericType(builder.RoleType);
    builder.Services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(builder.UserType), userStoreType);
    builder.Services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(builder.RoleType), roleStoreType);
    
    return builder;
  }
}