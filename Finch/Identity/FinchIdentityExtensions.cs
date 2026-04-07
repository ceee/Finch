using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Finch.Identity;

public static class FinchIdentityExtensions
{
  // <summary>
  /// Adds the default identity system configuration for the specified User and Role types.
  /// </summary>
  /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
  /// <typeparam name="TRole">The type representing a Role in the system.</typeparam>
  /// <param name="services">The services available in the application.</param>
  /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
  public static IdentityBuilder AddFinchIdentity<TUser, TRole>(this IServiceCollection services)
    where TUser : FinchIdentityUser, new()
    where TRole : FinchIdentityRole, new() =>
    AddFinchIdentity<TUser, TRole>(services, null);

  
  /// <summary>
  /// Adds and configures the identity system for the specified User and Role types.
  /// </summary>
  /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
  /// <typeparam name="TRole">The type representing a Role in the system.</typeparam>
  /// <param name="services">The services available in the application.</param>
  /// <param name="setupAction">An action to configure the <see cref="IdentityOptions"/>.</param>
  /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
  public static IdentityBuilder AddFinchIdentity<TUser, TRole>(this IServiceCollection services,
    Action<IdentityOptions> setupAction)
    where TUser : FinchIdentityUser, new()
    where TRole : FinchIdentityRole, new()
  {
    // Services identity depends on
    services
      .AddOptions()
      .AddLogging();
    
    // Services used by identity
    services.AddAuthentication(options =>
      {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
        options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
      })
      .AddCookie(IdentityConstants.ApplicationScheme, o =>
      {
        o.LoginPath = new PathString("/account/login");
        o.SlidingExpiration = true;
        o.ExpireTimeSpan = TimeSpan.FromDays(90);
        
        o.Cookie.Name = FinchIdentityConstants.CookieNames.Application;
        o.Cookie.HttpOnly = true;
        o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        o.Cookie.SameSite = SameSiteMode.Lax;
        o.Cookie.Path = "/";
        
        o.Events = new CookieAuthenticationEvents
        {
          OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
        };
      })
      .AddCookie(IdentityConstants.ExternalScheme, o =>
      {
        o.Cookie.Name = FinchIdentityConstants.CookieNames.External;
        o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
      })
      .AddCookie(IdentityConstants.TwoFactorRememberMeScheme, o =>
      {
        o.Cookie.Name = FinchIdentityConstants.CookieNames.TwoFactorRememberMe;
        o.Events = new CookieAuthenticationEvents
        {
          OnValidatePrincipal = SecurityStampValidator.ValidateAsync<ITwoFactorSecurityStampValidator>
        };
      })
      .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
      {
        o.Cookie.Name = FinchIdentityConstants.CookieNames.TwoFactorUserId;
        o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
      });

    // Hosting doesn't add IHttpContextAccessor by default
    services.AddHttpContextAccessor();
    
    // Identity services
    services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
    services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
    services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
    services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
    services.TryAddScoped<IRoleValidator<TRole>, RoleValidator<TRole>>();
    
    // No interface for the error describer so we can add errors without rev'ing the interface
    services.TryAddScoped<IdentityErrorDescriber>();
    services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<TUser>>();
    services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<TUser>>();
    services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser, TRole>>();
    services.TryAddScoped<IUserConfirmation<TUser>, DefaultUserConfirmation<TUser>>();
    services.TryAddScoped<UserManager<TUser>>();
    services.TryAddScoped<SignInManager<TUser>>();
    services.TryAddScoped<RoleManager<TRole>>();

    services.Configure<IdentityOptions>(opts =>
    {
      opts.ClaimsIdentity.UserIdClaimType = FinchIdentityConstants.Claims.UserId;
      opts.ClaimsIdentity.UserNameClaimType = FinchIdentityConstants.Claims.Username;
      opts.ClaimsIdentity.RoleClaimType = FinchIdentityConstants.Claims.Role;
      opts.ClaimsIdentity.SecurityStampClaimType = FinchIdentityConstants.Claims.SecurityStamp;
      opts.ClaimsIdentity.EmailClaimType = FinchIdentityConstants.Claims.Email;

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
    
    services.Configure<SecurityStampValidatorOptions>(opts => { opts.ValidationInterval = TimeSpan.FromMinutes(90); });

    if (setupAction != null)
    {
      services.Configure(setupAction);
    }

    IdentityBuilder builder = new(typeof(TUser), typeof(TRole), services);

    builder.AddDefaultTokenProviders();
    builder.AddFinchIdentityStores();
    
    return builder;
  }
  
  
  /// <summary>
  /// Configures the application cookie.
  /// </summary>
  /// <param name="services">The services available in the application.</param>
  /// <param name="configure">An action to configure the <see cref="CookieAuthenticationOptions"/>.</param>
  /// <returns>The services.</returns>
  public static IServiceCollection ConfigureFinchApplicationCookie(this IServiceCollection services, Action<CookieAuthenticationOptions> configure)
    => services.Configure(IdentityConstants.ApplicationScheme, configure);

  /// <summary>
  /// Configure the external cookie.
  /// </summary>
  /// <param name="services">The services available in the application.</param>
  /// <param name="configure">An action to configure the <see cref="CookieAuthenticationOptions"/>.</param>
  /// <returns>The services.</returns>
  public static IServiceCollection ConfigureFinchExternalCookie(this IServiceCollection services, Action<CookieAuthenticationOptions> configure)
    => services.Configure(IdentityConstants.ExternalScheme, configure);
}