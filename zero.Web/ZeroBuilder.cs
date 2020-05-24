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
using Raven.Client.Documents.Indexes;
using System;
using System.Reflection;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Core.Mapper;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Core.Validation;
using zero.Web.Defaults;

namespace zero.Web
{
  // TODO maybe use a middleware like Hangfire does: https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.AspNetCore/HangfireEndpointRouteBuilderExtensions.cs

  public class ZeroBuilder
  {
    public virtual IServiceCollection Services { get; }

    IConfiguration Configuration { get; set; }


    public ZeroBuilder(IServiceCollection services, IConfiguration configuration)
    {
      Services = services;
      Configuration = configuration;

      //CultureInfo cultureInfo = new CultureInfo("en-US");
      //cultureInfo.NumberFormat.CurrencySymbol = "€";
      //CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

      AddPlugin<DefaultBackofficePlugin>();

      AddConfiguration();
      ConfgureMvc();
      ConfigureDatabase();
      ConfigureValidation();
      ConfigureIdentity();
      AddServices();
      //AddPlugins();
    }


    /// <summary>
    /// Adds zero specific configuration
    /// </summary>
    void AddConfiguration()
    {
      Services.AddOptions<ZeroOptions>()
        .Bind(Configuration.GetSection("Zero"))
        .Configure(opts =>
        {
          opts.ZeroVersion = "0.0.1.0"; // TODO
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

      Services.AddMapper();
      Services.AddZeroCoreServices();

      Services.AddScoped<IApplicationContext, ApplicationContext>();

      Services.AddTransient<IBackofficeStore, BackofficeStore>();
      Services.AddTransient(typeof(IAppScope<>), typeof(AppScope<>));

      Services.AddTransient<ISetupApi, SetupApi>();
      Services.AddTransient<ISectionsApi, SectionsApi>();
      Services.AddTransient<IPagesApi, PagesApi>();
      Services.AddTransient<IPageTreeApi, PageTreeApi>();
      Services.AddTransient<ISettingsApi, SettingsApi>();
      Services.AddTransient<IAuthenticationApi, AuthenticationApi>();
      Services.AddTransient<IUserApi, UserApi>();
      Services.AddTransient<IUserRolesApi, UserRolesApi>();
      Services.AddTransient<IToken, Token>();
      Services.AddTransient<ISpacesApi, SpacesApi>();
      Services.AddTransient<ITranslationsApi, TranslationsApi>();
      Services.AddTransient<ILanguagesApi, LanguagesApi>();
      Services.AddTransient<IPermissionsApi, PermissionsApi>();
      Services.AddTransient<IMediaApi, MediaApi>();
      Services.AddTransient<IMediaFolderApi, MediaFolderApi>();
      Services.AddTransient<IMediaUpload, MediaUpload>();
    }


    /// <summary>
    /// Configures ASP.NET Core MVC
    /// </summary>
    IMvcBuilder ConfgureMvc()
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

      mvc.ConfigureApplicationPartManager(setup =>
      {
        setup.FeatureProviders.Add(new ApiControllerFeatureProvider());
      });

      return mvc;
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

        IDocumentStore raven = store.Setup(options).Initialize();

        // create all indexes
        IndexCreation.CreateIndexes(Assembly.GetAssembly(typeof(MediaFolder_ByHierarchy)), store);

        return raven;
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


    /// <summary>
    /// Use specified options
    /// </summary>
    public ZeroBuilder WithOptions(Action<ZeroOptions> configureOptions)
    {
      Services.Configure(configureOptions);
      return this;
    }


    private static EntityDefinition[] Entities = new EntityDefinition[] { };

    //public ZeroPlugin Use<TService, TImplementation>(Action<EntityDefinition<TService, TImplementation>> configure) where TImplementation : TService
    //{
    //  //configure()
    //}

    //public ZeroPlugin Use<TService, TImplementation>() where TImplementation : TService
    //{
    //  //configure()
    //}


    /// <summary>
    /// Adds a zero plugin
    /// </summary>
    public void AddPlugin<T>() where T : class, IZeroPlugin, new()
    {
      Services.AddScoped<IZeroPlugin, T>();
      AddPluginServices<T>();
    }


    /// <summary>
    /// Adds a zero plugin
    /// </summary>
    public void AddPlugin<T>(Func<IServiceProvider, T> implementationFactory) where T : class, IZeroPlugin, new()
    {
      Services.AddScoped<IZeroPlugin, T>(implementationFactory);
      AddPluginServices<T>();
    }


    /// <summary>
    /// Creates a temporary instance of the plugin to add additional services
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void AddPluginServices<T>() where T : class, IZeroPlugin, new()
    {
      try
      {
        T plugin = new T();

        plugin.ConfigureServices(Services);

        Services.Configure<ZeroOptions>(opts => plugin.Configure(new ZeroPluginOptions(), opts));
      }
      catch
      {
        throw new Exception($"Plugin \"{nameof(T)}\" needs an additional parameterless constructor as ConfigureServices() is called before the DI container is built");
      }
    }
  }
}
