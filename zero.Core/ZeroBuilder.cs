using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero;

// TODO maybe use a middleware like Hangfire does: https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.AspNetCore/HangfireEndpointRouteBuilderExtensions.cs

public class ZeroBuilder
{
  public virtual IServiceCollection Services { get; }

  public virtual IMvcBuilder Mvc { get; }

  internal static ZeroModuleCollection Modules { get; private set; } = new();

  readonly IConfiguration Configuration;
  readonly IZeroStartupOptions StartupOptions;

  internal static HashSet<IZeroPlugin> Plugins { get; private set; } = new();


  public ZeroBuilder(IServiceCollection services, IConfiguration configuration, Action<IZeroStartupOptions> setupAction)
  {
    Services = services;
    Mvc = services.AddMvc();
    Configuration = configuration;

    // create startup options
    StartupOptions = new ZeroStartupOptions(Mvc);
    StartupOptions.AssemblyDiscoveryRules.Add(new ZeroAssemblyDiscoveryRule());
    setupAction?.Invoke(StartupOptions);

    services.AddControllers();
    services.AddRazorPages();

    // adds and discovers additional and built-in assemblies
    new AssemblyDiscovery(Mvc).Execute(StartupOptions.AssemblyDiscoveryRules);

    Modules.Add<ZeroApplicationModule>();
    Modules.Add<ZeroArchitectureModule>();
    Modules.Add<ZeroCommunicationModule>();
    Modules.Add<ZeroConfigurationModule>();
    Modules.Add<ZeroValidationModule>();
    Modules.Add<ZeroContextModule>();
    Modules.Add<ZeroFileStorageModule>();
    Modules.Add<ZeroIdentityModule>();
    Modules.Add<ZeroLocalizationModule>();
    Modules.Add<ZeroMailModule>();
    Modules.Add<ZeroMapperModule>();
    Modules.Add<ZeroMediaModule>();
    Modules.Add<ZeroPageModule>();
    Modules.Add<ZeroPersistenceModule>();
    Modules.Add<ZeroRenderingModule>();
    Modules.Add<ZeroRoutingModule>();
    Modules.Add<ZeroSpaceModule>();
    Modules.Add<ZeroStoreModule>();
    Modules.Add<ZeroSearchModule>();

    Modules.ConfigureServices(services, configuration);

    // configure FluentValidation
    ValidatorOptions.Global.PropertyNameResolver = ValidatorCamelCasePropertyResolver.ResolvePropertyName;
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
    T plugin = new T();
    plugin.ConfigureServices(Services, Configuration);
    AssemblyDiscovery.Current.AddAssembly(typeof(T).Assembly);
    Services.AddSingleton<IZeroPlugin, T>();
    Plugins.Add(plugin);
    return this;
  }
}
