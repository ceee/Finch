using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Security.Claims;
using zero.Core.Entities;
using zero.Core.Security;

namespace zero.Core.Identity
{
  public static class ZeroIdentityExtensions
  {
    public static IdentityBuilder AddZeroIdentity<TUser, TRole>(this IServiceCollection services, Action<IdentityOptions> setupAction = null) 
      where TUser : class, IUser
      where TRole : class, IUserRole
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

      IdentityBuilder builder = new IdentityBuilder(typeof(TUser), services);

      builder.AddDefaultTokenProviders();
      builder.AddUserStore<UserStore<TUser>>();
      builder.AddUserManager<UserManager<TUser>>();
      builder.AddSignInManager<SignInManager<TUser>>();
      builder.AddClaimsPrincipalFactory<ZeroClaimsPrinicipalFactory<TUser, TRole>>();

      builder.AddRoles<TRole>();
      builder.AddRoleManager<RoleManager<TRole>>();
      builder.AddRoleStore<RoleStore<TRole>>();

      return builder;
    }
  }
}
