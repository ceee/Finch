using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Core.Mapper;
using zero.Core.Plugins;
using zero.Core.Validation;
using zero.Web.Mapper;

namespace zero.Web
{
  // TODO maybe use a middleware like Hangfire does: https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.AspNetCore/HangfireEndpointRouteBuilderExtensions.cs

  public class ZeroBuilder
  {
    public virtual IServiceCollection Services { get; }

    public virtual ZeroOptions Options { get; }

    IConfiguration Configuration { get; set; }


    public ZeroBuilder(IServiceCollection services, IConfiguration configuration)
    {
      Services = services;
      Configuration = configuration;

      //CultureInfo cultureInfo = new CultureInfo("en-US");
      //cultureInfo.NumberFormat.CurrencySymbol = "€";
      //CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

      AddConfiguration();
      ConfgureMvc();
      ConfigureDatabase();
      ConfigureValidation();
      ConfigureMapper();
      ConfigureIdentity();
      AddServices();
      AddPlugins();
    }


    /// <summary>
    /// Adds zero specific configuration
    /// </summary>
    void AddConfiguration()
    {
      Services.Configure<ZeroOptions>(Configuration.GetSection("Zero"));
      Services.PostConfigure<ZeroOptions>(opts =>
      {
        opts.ZeroVersion = "0.0.1.0"; // TODO
        opts.Backoffice = new DefaultBackofficePlugin();
      });
      Services.AddTransient<IZeroOptions>(factory => factory.GetService<IOptionsMonitor<ZeroOptions>>().CurrentValue);
    }


    /// <summary>
    /// Adds all services which are required by zero
    /// </summary>
    void AddServices()
    {
      Services.AddHttpContextAccessor();

      Services.AddTransient<IZeroVue, ZeroVue>();
      Services.AddTransient<IPaths>(factory =>
      {
        IWebHostEnvironment env = factory.GetService<IWebHostEnvironment>();
        return new Paths(env.WebRootPath, true);
      });

      Services.AddTransient<IBackofficeStore, BackofficeStore>();
      Services.AddTransient<IAppAwareBackofficeStore, AppAwareBackofficeStore>();

      Services.AddTransient<ISetupApi, SetupApi>();
      Services.AddTransient<ISectionsApi, SectionsApi>();
      Services.AddTransient<IApplicationsApi, ApplicationsApi>();
      Services.AddTransient<IPagesApi, PagesApi>();
      Services.AddTransient<IPageTreeApi, PageTreeApi>();
      Services.AddTransient<ISettingsApi, SettingsApi>();
      Services.AddTransient<IAuthenticationApi, AuthenticationApi>();
      Services.AddTransient<ICountriesApi, CountriesApi>();
      Services.AddTransient<IUserApi, UserApi>();
      Services.AddTransient<IUserRolesApi, UserRolesApi>();
      Services.AddTransient<IToken, Token>();
      Services.AddTransient<ISpacesApi, SpacesApi>();
      Services.AddTransient<ITranslationsApi, TranslationsApi>();
      Services.AddTransient<ILanguagesApi, LanguagesApi>();
      Services.AddTransient<IPermissionsApi, PermissionsApi>();
      Services.AddTransient<IMediaApi, MediaApi>();
      Services.AddTransient<IMediaUpload, MediaUpload>();

      Services.AddSingleton<IZeroPluginBuilder, ZeroPluginBuilder>();
    }


    /// <summary>
    /// Configures ASP.NET Core MVC
    /// </summary>
    void ConfgureMvc()
    {
      IMvcBuilder mvc = Services.AddMvc();

      mvc.AddNewtonsoftJson(opts =>
      {
        opts.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss'Z'" });
        opts.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
        opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        JsonConvert.DefaultSettings = () => opts.SerializerSettings;
      });

      if (Environment.GetEnvironmentVariable("DOTNET_WATCH") == "1")
      {
        mvc.AddRazorRuntimeCompilation();
      }
    }


    /// <summary>
    /// Configures Raven database instance
    /// </summary>
    void ConfigureDatabase()
    {
      // add raven
      Services.AddSingleton(context =>
      {
        IZeroOptions options = context.GetService<IZeroOptions>();
         
        DocumentStore store = new DocumentStore()
        {
          Urls = new string[1] { options.Raven.Url },
          Database = options.Raven.Database
        };

        store.Conventions.FindCollectionName = type =>
        {
          if (!typeof(IZeroDbConventions).IsAssignableFrom(type))
          {
            return DocumentConventions.DefaultGetCollectionName(type);
          }

          Type finalType = type;

          if (type.IsSubclassOf(typeof(SpaceContent)))
          {
            finalType = typeof(SpaceContent);
          }

          return Constants.Database.CollectionPrefix + DocumentConventions.DefaultGetCollectionName(finalType);
        };

        store.Conventions.TransformTypeCollectionNameToDocumentIdPrefix = name =>
        {
          return name.ToCamelCaseId();
        };

        store.Conventions.IdentityPartsSeparator = ".";

        return store.Initialize();
      });
    }


    /// <summary>
    /// Configures FluentValidation
    /// </summary>
    void ConfigureValidation()
    {
      ValidatorOptions.PropertyNameResolver = ValidatorCamelCasePropertyResolver.ResolvePropertyName;
    }


    /// <summary>
    /// Configures internal object mapper
    /// </summary>
    void ConfigureMapper()
    {
      Services.AddMapper(opts =>
      {
        opts.Add<UserMapperConfig>();
        opts.Add<CountryMapperConfig>();
        opts.Add<TranslationMapperConfig>();
        opts.Add<LanguageMapperConfig>();
        opts.Add<ApplicationMapperConfig>();
      });
    }


    /// <summary>
    /// Configures user + roles
    /// </summary>
    void ConfigureIdentity()
    {
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
    }


    void AddPlugins()
    {
      AddPlugin(x => x.GetService<IZeroOptions>().Backoffice);

      // build plugins
      //IEnumerable<IZeroPlugin> plugins = app.ApplicationServices.GetServices<IZeroPlugin>();
      //IZeroPluginBuilder pluginBuilder = app.ApplicationServices.GetService<IZeroPluginBuilder>();

      //foreach (IZeroPlugin plugin in plugins)
      //{
      //  plugin.Configure(app.ApplicationServices)
      //}
    }


    /// <summary>
    /// Use specified options
    /// </summary>
    public ZeroBuilder WithOptions(Action<ZeroOptions> configureOptions)
    {
      Services.PostConfigure(configureOptions);
      return this;
    }


    /// <summary>
    /// Adds a zero plugin
    /// </summary>
    public ZeroBuilder AddPlugin<T>() where T : class, IZeroPlugin
    {
      Services.AddTransient<IZeroPlugin, T>();
      return this;
    }


    /// <summary>
    /// Adds a zero plugin
    /// </summary>
    public ZeroBuilder AddPlugin<T>(Func<IServiceProvider, T> implementationFactory) where T : class, IZeroPlugin
    {
      Services.AddTransient<IZeroPlugin, T>(implementationFactory);
      return this;
    }
  }
}
