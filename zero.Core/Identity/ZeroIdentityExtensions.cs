using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace zero;

public static class ZeroIdentityExtensions
{
  public static IdentityBuilder AddZeroIdentity<TUser, TRole>(this IServiceCollection services) 
    where TUser : ZeroIdentityUser
    where TRole : ZeroIdentityRole
  {
    services.AddZeroIdentityCore<TUser>();

    IdentityBuilder builder = new IdentityBuilder(typeof(TUser), typeof(TRole), services);

    builder.AddDefaultTokenProviders();

    builder.AddUserStore<RavenUserStore<TUser, TRole>>();
    builder.AddSignInManager<SchemedSignInManager<TUser>>();
    builder.AddClaimsPrincipalFactory<ZeroClaimsPrinicipalFactory<TUser, TRole>>();

    builder.AddRoleValidator<RoleValidator<TRole>>();
    builder.AddRoleManager<RoleManager<TRole>>();
    builder.AddRoleStore<RavenRoleStore<TRole>>();

    return builder;
  }


  public static IdentityBuilder AddZeroIdentity<TUser>(this IServiceCollection services)
    where TUser : ZeroIdentityUser
  {
    services.AddZeroIdentityCore<TUser>();

    IdentityBuilder builder = new IdentityBuilder(typeof(TUser), services);

    builder.AddDefaultTokenProviders();
    builder.AddUserStore<RavenUserStore<TUser>>();
    builder.AddSignInManager<SchemedSignInManager<TUser>>();
    builder.AddClaimsPrincipalFactory<ZeroClaimsPrinicipalFactory<TUser>>();

    return builder;
  }


  static IServiceCollection AddZeroIdentityCore<TUser>(this IServiceCollection services)
    where TUser : ZeroIdentityUser
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
      opts.ValidationInterval = TimeSpan.FromMinutes(90);
    });

    return services;
  }
}