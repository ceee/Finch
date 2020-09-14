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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Assemblies;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Core.Mapper;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Core.Utils;
using zero.Core.Validation;
using zero.Web.Defaults;
using zero.Web.Filters;

namespace zero.Web
{
  // TODO maybe use a middleware like Hangfire does: https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.AspNetCore/HangfireEndpointRouteBuilderExtensions.cs

  public class ZeroBuilder
  {
    public virtual IServiceCollection Services { get; }

    public virtual IMvcBuilder Mvc { get; }

    IConfiguration Configuration;

    IZeroStartupOptions StartupOptions;


    public ZeroBuilder(IServiceCollection services, IConfiguration configuration, Action<IZeroStartupOptions> setupAction)
    {
      Services = services;
      Mvc = services.AddMvc(opts =>
      {
        //opts.ModelBinderProviders.Insert(0, new ZeroEntityInterfaceBinderProvider());
      });
      Configuration = configuration;

      // create startup options
      StartupOptions = new ZeroStartupOptions(Mvc);
      StartupOptions.AssemblyDiscoveryRules.Add(new ZeroAssemblyDiscoveryRule());
      setupAction?.Invoke(StartupOptions);


      // adds and discovers additional and built-in assemblies
      new AssemblyDiscovery(Mvc).Execute(StartupOptions.AssemblyDiscoveryRules);


      // add default plugin
      AddPlugin<ZeroBackofficePlugin>();


      // create and bind zero options
      Services.AddOptions<ZeroOptions>()
        .Bind(Configuration.GetSection("Zero"))
        .Configure(opts =>
        {
          //opts.AssemblyDiscoveryRules.Add(new ZeroAssemblyDiscoveryRule());
          opts.ZeroVersion = "0.0.1.0"; // TODO
        });


      // add transient options to DI
      Services.AddTransient<IZeroOptions>(factory => factory.GetService<IOptionsMonitor<ZeroOptions>>().CurrentValue);


      // configure MVC
      Mvc.AddNewtonsoftJson(x =>
      {
        // TODO this shall only be configurated for backoffice controllers
        x.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
        x.SerializerSettings.ContractResolver = new ZeroJsonContractResolver();
        x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        x.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
      });
      if (Environment.GetEnvironmentVariable("DOTNET_WATCH") == "1")
      {
        Mvc.AddRazorRuntimeCompilation();
      }


      // configure Raven + Identity
      ConfigureDatabase();
      ConfigureIdentity();


      // configure FluentValidation
      ValidatorOptions.Global.PropertyNameResolver = ValidatorCamelCasePropertyResolver.ResolvePropertyName;


      // add default mapper
      Services.AddMapper();


      // add default services
      Services.AddScoped<IApplicationContext, ApplicationContext>();

      Services.AddTransient<IBackofficeStore, BackofficeStore>();
      Services.AddTransient(typeof(IAppScope<>), typeof(AppScope<>));

      Services.AddScoped<ModelStateValidationFilterAttribute>();

      Services.AddHttpContextAccessor();

      Services.AddTransient<IZeroVue, ZeroVue>();
      Services.AddTransient<IPaths>(factory =>
      {
        IWebHostEnvironment env = factory.GetService<IWebHostEnvironment>();
        return new Paths(env.WebRootPath, true);
      });
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
        var assemblies = AssemblyDiscovery.Current.GetAssemblies().ToList();

        // TODO maybe we shouldn't use all auto-registered assemblies but specify them directly via options?
        foreach (Assembly assembly in assemblies)
        {
          IndexCreation.CreateIndexes(assembly, store);
        }

        return raven;
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


    /// <summary>
    /// Use specified options
    /// </summary>
    public ZeroBuilder WithOptions(Action<ZeroOptions> configureOptions)
    {
      Services.Configure(configureOptions);
      return this;
    }


    /// <summary>
    /// Adds a zero plugin
    /// </summary>
    public ZeroBuilder AddPlugin<T>() where T : class, IZeroPlugin, new()
    {
      AssemblyDiscovery.Current.AddAssembly(typeof(T).Assembly);
      Services.AddScoped<IZeroPlugin, T>();
      AddPluginServices<T>();
      return this;
    }


    /// <summary>
    /// Adds a zero plugin
    /// </summary>
    public ZeroBuilder AddPlugin<T>(Func<IServiceProvider, T> implementationFactory) where T : class, IZeroPlugin, new()
    {
      AssemblyDiscovery.Current.AddAssembly(typeof(T).Assembly);
      Services.AddScoped<IZeroPlugin, T>(implementationFactory);
      AddPluginServices<T>();
      return this;
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

        plugin.ConfigureServices(Services, Configuration);

        Services.Configure<ZeroOptions>(opts => plugin.Configure(new ZeroPluginOptions(), opts));
      }
      catch
      {
        throw new Exception($"Plugin \"{nameof(T)}\" needs an additional parameterless constructor as ConfigureServices() is called before the DI container is built");
      }
    }
  }
}
