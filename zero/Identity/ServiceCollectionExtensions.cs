using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using zero.Identity;

namespace zero.Identity;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the default identity system configuration for the specified User and Role types
    /// </summary>
    // public static IdentityBuilder AddZeroIdentity<TUser, TRole, TStore>(
    //     this IServiceCollection services)
    //     where TUser : ZeroIdentityUser, new()
    //     where TRole : ZeroIdentityRole, new()
    //     where TStore : class, IZeroIdentityStoreDbProvider 
    //     => services.AddIdentity<TUser, TRole>(setupAction: null!);

    /// <summary>
    /// Adds and configures the identity system for the specified User and Role types
    /// </summary>
    public static IdentityBuilder AddZeroIdentity<TUser, TRole, TStore>(
        this IServiceCollection services,
        Action<IdentityOptions> setupAction
    )
        where TUser : ZeroIdentityUser, new()
        where TRole : ZeroIdentityRole, new()
        where TStore : class, IZeroIdentityStoreDbProvider 
    {
        // Services used by identity
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddCookie(IdentityConstants.ApplicationScheme, o =>
        {
            o.LoginPath = new PathString("/Account/Login");
            o.Events = new CookieAuthenticationEvents
            {
                OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
            };
        })
        .AddCookie(IdentityConstants.ExternalScheme, o =>
        {
            o.Cookie.Name = IdentityConstants.ExternalScheme;
            o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        })
        .AddCookie(IdentityConstants.TwoFactorRememberMeScheme, o =>
        {
            o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
            o.Events = new CookieAuthenticationEvents
            {
                OnValidatePrincipal = SecurityStampValidator.ValidateAsync<ITwoFactorSecurityStampValidator>
            };
        })
        .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
        {
            o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
            o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        });

        // Hosting doesn't add IHttpContextAccessor by default
        services.AddHttpContextAccessor();

        // Data stores
        services.AddScoped<IZeroIdentityStoreDbProvider, TStore>();
        services.TryAddScoped<IUserStore<TUser>, ZeroUserStore<TUser>>();
        services.TryAddScoped<IRoleStore<TRole>, ZeroRoleStore<TRole>>();

        // Identity services
        services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
        services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
        services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
        services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
        services.TryAddScoped<IRoleValidator<TRole>, RoleValidator<TRole>>();
        services.TryAddScoped<IdentityErrorDescriber>();
        services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<TUser>>();
        services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<TUser>>();
        services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser, TRole>>();
        services.TryAddScoped<IUserConfirmation<TUser>, DefaultUserConfirmation<TUser>>();
        services.TryAddScoped<UserManager<TUser>>();
        services.TryAddScoped<SignInManager<TUser>>();
        services.TryAddScoped<RoleManager<TRole>>();

        if (setupAction != null)
        {
            services.Configure(setupAction);
        }

        return new IdentityBuilder(typeof(TUser), typeof(TRole), services);
    }
}