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


    services.AddZeroApplications(Configuration);
    services.AddZeroBlueprints(Configuration);
    services.AddZeroCommunication();
    services.AddZeroConfiguration(Configuration);
    services.AddZeroContext();
    services.AddZeroFileStorage(Configuration);
    services.AddZeroIdentity();
    services.AddZeroLocalization();
    services.AddZeroMails();
    services.AddZeroPages(Configuration);
    services.AddZeroMedia(Configuration);
    services.AddZeroPersistence(Configuration);
    services.AddZeroRendering();
    services.AddZeroRouting(Configuration);
    services.AddZeroStores();

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
