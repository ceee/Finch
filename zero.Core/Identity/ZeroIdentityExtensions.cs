using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using zero.Core.Entities;
using zero.Core.Security;

namespace zero.Core.Identity
{
  public static class ZeroIdentityExtensions
  {
    public static IdentityBuilder AddZeroBackofficeIdentity<TUser, TRole>(this IServiceCollection services, Action<IdentityOptions> setupAction = null)
      where TUser : class, IUser
      where TRole : class, IUserRole
    {
      IdentityBuilder builder = services.AddZeroIdentity<TUser, TRole>(setupAction);
      services.RemoveAll<IUserClaimsPrincipalFactory<TUser>>();
      builder.AddClaimsPrincipalFactory<ZeroBackofficeClaimsPrincipalFactory<TUser, TRole>>();
      return builder;
    }

    public static IdentityBuilder AddZeroIdentity<TUser, TRole>(this IServiceCollection services, Action<IdentityOptions> setupAction = null) 
      where TUser : class, IIdentityUserWithRoles, IIdentityUser
      where TRole : class, IIdentityUserRole
    {
      services.AddZeroIdentityCore<TUser>(setupAction);

      IdentityBuilder builder = new IdentityBuilder(typeof(TUser), typeof(TRole), services);

      builder.AddDefaultTokenProviders();
      builder.AddUserStore<UserStore<TUser>>();
      builder.AddUserManager<UserManager<TUser>>();
      builder.AddSignInManager<ZeroSignInManager<TUser>>();
      builder.AddClaimsPrincipalFactory<ZeroClaimsPrinicipalFactory<TUser, TRole>>();
      builder.AddRoleValidator<RoleValidator<TRole>>();
      builder.AddRoleManager<RoleManager<TRole>>();
      builder.AddRoleStore<RoleStore<TRole>>();

      return builder;
    }


    public static IdentityBuilder AddZeroIdentity<TUser>(this IServiceCollection services, Action<IdentityOptions> setupAction = null)
      where TUser : class, IIdentityUser
    {
      services.AddZeroIdentityCore<TUser>(setupAction);

      IdentityBuilder builder = new IdentityBuilder(typeof(TUser), services);

      builder.AddDefaultTokenProviders();
      builder.AddUserStore<UserStore<TUser>>();
      builder.AddUserManager<UserManager<TUser>>();
      builder.AddSignInManager<ZeroSignInManager<TUser>>();
      builder.AddClaimsPrincipalFactory<ZeroClaimsPrinicipalFactory<TUser>>();

      return builder;
    }


    static IServiceCollection AddZeroIdentityCore<TUser>(this IServiceCollection services, Action<IdentityOptions> setupAction = null)
      where TUser : class, IIdentityUser
    {
      services.AddHttpContextAccessor();
      services.AddOptions();
      services.AddLogging();

      services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
      services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
      services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
      services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
      services.TryAddScoped<IUserConfirmation<TUser>, DefaultUserConfirmation<TUser>>();
      services.TryAddScoped<IdentityErrorDescriber>();
      services.TryAddScoped<UserManager<TUser>>();

      services.Configure<IdentityOptions>(opts =>
      {
        opts.ClaimsIdentity.UserIdClaimType = Constants.Auth.Claims.UserId;
        opts.ClaimsIdentity.UserNameClaimType = Constants.Auth.Claims.UserName;
        opts.ClaimsIdentity.RoleClaimType = Constants.Auth.Claims.Role;
        opts.ClaimsIdentity.SecurityStampClaimType = Constants.Auth.Claims.SecurityStamp;
        opts.ClaimsIdentity.EmailClaimType = Constants.Auth.Claims.Email;

        opts.Password.RequireDigit = false;
        opts.Password.RequireLowercase = false;
        opts.Password.RequireUppercase = false;
        opts.Password.RequireNonAlphanumeric = false;
        opts.Password.RequiredLength = 8;
        opts.Password.RequiredUniqueChars = 1;

        opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
        opts.Lockout.MaxFailedAccessAttempts = 5;
        opts.Lockout.AllowedForNewUsers = true;

        opts.User.RequireUniqueEmail = true;
      });

      services.Configure<SecurityStampValidatorOptions>(opts =>
      {
        opts.ValidationInterval = TimeSpan.FromMinutes(30);
      });

      if (setupAction != null)
      {
        services.Configure(setupAction);
      }

      return services;
    }
  }
}
