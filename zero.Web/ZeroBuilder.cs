using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Assemblies;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Core.Security;
using zero.Core.Validation;
using zero.Web.Controllers;
using zero.Web.Defaults;
using zero.Web.Filters;
using zero.Web.Security;
using Zero.Web.DevServer;

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
      Mvc = services.AddMvc();
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
      Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, ZeroBuilderMvcOptions>());

      Mvc.AddNewtonsoftJson(x =>
      {
        // TODO this shall only be configurated for backoffice controllers
        BackofficeJsonSerlializerSettings.Setup(x.SerializerSettings);
      });

      if (Environment.GetEnvironmentVariable("DOTNET_WATCH") == "1")
      {
        Mvc.AddRazorRuntimeCompilation();
      } 

      // configure Raven + identity
      ConfigureDatabase();
      ConfigureIdentity();

      // configure FluentValidation
      ValidatorOptions.Global.PropertyNameResolver = ValidatorCamelCasePropertyResolver.ResolvePropertyName;


      // add default services
      Services.AddHttpContextAccessor();
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


      // add dev server
      Services.AddOptions<ZeroDevOptions>()
        .Bind(Configuration.GetSection("Zero:DevServer"))
        .Configure<IWebHostEnvironment>((opts, env) =>
        {
          opts.WorkingDirectory = Path.Combine(env.ContentRootPath, "..", "Zero.Web.UI", "App");
        });

      Services.AddHostedService<ZeroDevService>();
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
    /// Configures identity
    /// </summary>
    void ConfigureIdentity()
    {
      Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>());

      Services.AddZeroIdentity<User, UserRole>();
      Services.Replace<IUserClaimsPrincipalFactory<User>, ZeroBackofficeClaimsPrincipalFactory<User, UserRole>>();

      Services.AddAuthentication()
        .AddZeroBackofficeCookie<User, UserRole>();

      Services.AddAuthorization();
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
      Services.AddSingleton<IZeroPlugin, T>();
      AddPluginServices<T>();
      return this;
    }


    /// <summary>
    /// Adds a zero plugin
    /// </summary>
    public ZeroBuilder AddPlugin<T>(Func<IServiceProvider, T> implementationFactory) where T : class, IZeroPlugin, new()
    {
      AssemblyDiscovery.Current.AddAssembly(typeof(T).Assembly);
      Services.AddSingleton<IZeroPlugin, T>(implementationFactory);
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

        Services.Configure<ZeroOptions>(opts => plugin.Configure(opts));
      }
      catch
      {
        throw new Exception($"Plugin \"{nameof(T)}\" needs an additional parameterless constructor as ConfigureServices() is called before the DI container is built");
      }
    }


    class ZeroBuilderMvcOptions : IConfigureOptions<MvcOptions>
    {
      IZeroOptions Options { get; set; }

      public ZeroBuilderMvcOptions(IZeroOptions options)
      {
        Options = options;
      }

      public void Configure(MvcOptions options)
      {
        options.Conventions.Add(new ZeroBackofficeControllerModelConvention(Options.BackofficePath));
      }
    }
  }
}
