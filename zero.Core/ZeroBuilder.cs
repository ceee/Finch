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

  public ZeroModuleCollection Modules { get; } = new();

  readonly IConfiguration Configuration;
  readonly IZeroStartupOptions StartupOptions;


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

    // adds and discovers additional and built-in assemblies
    new AssemblyDiscovery(Mvc).Execute(StartupOptions.AssemblyDiscoveryRules);

    Modules.Add<ConfigurationModule>();
    Modules.Add<ContextModule>();
    Modules.Add<ArchitectureModule>();
    Modules.Add<CommunicationModule>();
    Modules.Add<IdentityModule>();
    Modules.Add<ApplicationModule>();
    Modules.Add<PersistenceModule>();
    Modules.Add<StoresModule>();
    Modules.Add<FileStorageModule>();
    Modules.Add<MapperModule>();
    Modules.Add<LocalizationModule>();
    Modules.Add<RenderingModule>();

    Modules.Add<RoutingModule>();
    Modules.Add<MailsModule>();
    Modules.Add<MediaModule>();
    Modules.Add<PagesModule>();
    Modules.Add<SpacesModule>();

    Modules.ConfigureServices(services, configuration);

    //if (Environment.GetEnvironmentVariable("DOTNET_WATCH") == "1")
    //{
    //  Mvc.AddRazorRuntimeCompilation();
    //} 

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
    ZeroPluginInitializer.AddPlugin<T>(Services, Configuration);
    return this;
  }


  /// <summary>
  /// Adds a zero plugin
  /// </summary>
  public ZeroBuilder AddPlugin<T>(Func<IServiceProvider, T> implementationFactory) where T : class, IZeroPlugin, new()
  {
    ZeroPluginInitializer.AddPlugin<T>(Services, Configuration, implementationFactory);
    return this;
  }
}
