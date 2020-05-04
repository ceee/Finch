using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Validation;
using zero.Web.Mapper;
using zero.Web.Sections;

namespace zero.Web
{
  public class ZeroBuilder
  {
    public virtual IServiceCollection Services { get; }

    public virtual ZeroOptions Options { get; }


    public ZeroBuilder(IServiceCollection services)
    {
      Services = services;

      ValidatorOptions.PropertyNameResolver = ValidatorCamelCasePropertyResolver.ResolvePropertyName;

      Services.AddOptions<ZeroOptions>().Configure(opts => ConfigureDefaults(opts));

      Services.AddMapper(opts =>
      {
        opts.Add<UserMapperConfig>();
        opts.Add<CountryMapperConfig>();
        opts.Add<TranslationMapperConfig>();
      });

      Services.AddIdentity<User, UserRole>(opts =>
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

        opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        opts.Lockout.MaxFailedAccessAttempts = 5;
        opts.Lockout.AllowedForNewUsers = true;

      })
        .AddClaimsPrincipalFactory<ZeroClaimsPrinicipalFactory>()
        .AddDefaultTokenProviders();

      Services.ConfigureApplicationCookie(opts =>
      {
        //opts.Cookie.Path = // TODO use backoffice path
        opts.Cookie.Name = Constants.Auth.CookieName;
        opts.SlidingExpiration = true;
        opts.ExpireTimeSpan = TimeSpan.FromMinutes(60);

        // override redirect to login page (handled by vue frontend) and return a 401 instead
        opts.Events.OnRedirectToLogin = (context) =>
        {
          context.Response.StatusCode = 401;
          return Task.CompletedTask;
        };
      });

      Services.AddTransient<IUserStore<User>, ZeroUserStore>();
      Services.AddTransient<IRoleStore<UserRole>, ZeroRoleStore>();

      Services.AddScoped<UserManager<User>>();
      Services.AddScoped<SignInManager<User>>();
      Services.AddScoped<RoleManager<UserRole>>();

      //services.AddAuthorization(opts =>
      //{
      //  opts.AddPolicy("zero.sections.dashboard", builder =>
      //  {
      //    //builder.RequireClaim()
      //  });
      //});
    }


    void ConfigureDefaults(ZeroOptions opts)
    {
      opts.BackofficePath = "/zero";

      opts.Sections.Add<DashboardSection>();
      opts.Sections.Add<PagesSection>(); 
      opts.Sections.Add<SpacesSection>();
      opts.Sections.Add<MediaSection>();
      opts.Sections.Add<SettingsSection>();

      SettingsGroup systemSettings = new SettingsGroup("@settings.groups.system");
      systemSettings.Add(Constants.Settings.Updates, "@settings.system.updates.name", "@settings.system.updates.text", "fth-check-circle");
      systemSettings.Add(Constants.Settings.Applications, "@settings.system.applications.name", "@settings.system.applications.text", "fth-layers");
      systemSettings.Add(Constants.Settings.Users, "@settings.system.users.name", "@settings.system.users.text", "fth-users");
      systemSettings.Add(Constants.Settings.Translations, "@settings.system.translations.name", "@settings.system.translations.text", "fth-type");
      systemSettings.Add(Constants.Settings.Countries, "@settings.system.countries.name", "@settings.system.countries.text", "fth-map-pin");
      systemSettings.Add(Constants.Settings.Logging, "@settings.system.logs.name", "@settings.system.logs.text", "fth-file-text");

      SettingsGroup pluginSettings = new SettingsGroup("@settings.groups.plugins");
      pluginSettings.Add(Constants.Settings.Plugins, "@settings.plugins.installed.name", "@settings.plugins.installed.text", "fth-package");
      pluginSettings.Add(Constants.Settings.CreatePlugin, "@settings.plugins.create.name", "@settings.plugins.create.text", "fth-box");

      opts.SettingsAreas.Add(systemSettings);
      opts.SettingsAreas.Add(pluginSettings);

      PermissionCollection permissionSettings = new PermissionCollection()
      {
        Label = "@permission.collections.settings",
        Description = "@permission.collections.settings_description"
      };

      permissionSettings.Items.Add(new Permission(Permissions.Settings.Updates, "@settings.system.updates.name", "@settings.system.updates.text_default", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Applications, "@settings.system.applications.name", "@settings.system.applications.text", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Users, "@settings.system.users.name", "@settings.system.users.text", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Translations, "@settings.system.translations.name", "@settings.system.translations.text", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Countries, "@settings.system.countries.name", "@settings.system.countries.text", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Logging, "@settings.system.logs.name", "@settings.system.logs.text", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Plugins, "@settings.plugins.installed.name", "@settings.plugins.installed.text_default", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.CreatePlugin, "@settings.plugins.create.name", "@settings.plugins.create.text", PermissionValueType.ReadWrite));

      PermissionCollection permissionSections = new PermissionCollection()
      {
        Label = "@permission.collections.sections",
        Description = "@permission.collections.sections_description"
      };

      permissionSections.Items.Add(new Permission(Permissions.Sections.Dashboard, "@sections.item.dashboard", null, PermissionValueType.Boolean));
      permissionSections.Items.Add(new Permission(Permissions.Sections.Pages, "@sections.item.pages", null, PermissionValueType.Boolean));
      permissionSections.Items.Add(new Permission(Permissions.Sections.Spaces, "@sections.item.spaces", null, PermissionValueType.Boolean));
      permissionSections.Items.Add(new Permission(Permissions.Sections.Media, "@sections.item.media", null, PermissionValueType.Boolean));
      permissionSections.Items.Add(new Permission(Permissions.Sections.Settings, "@sections.item.settings", null, PermissionValueType.Boolean));

      opts.Authorization.Permissions.Add(permissionSections);
      opts.Authorization.Permissions.Add(permissionSettings);
    }


    public ZeroBuilder WithOptions(Action<ZeroOptions> configureOptions)
    {
      Services.PostConfigure(configureOptions);
      return this;
    }


    //public virtual AuthenticationBuilder AddPolicyScheme(string authenticationScheme, string displayName, Action<PolicySchemeOptions> configureOptions);
   
    //public virtual AuthenticationBuilder AddRemoteScheme<TOptions, THandler>(string authenticationScheme, string displayName, Action<TOptions> configureOptions)
    //  where TOptions : RemoteAuthenticationOptions, new()
    //  where THandler : RemoteAuthenticationHandler<TOptions>;
  }
}
