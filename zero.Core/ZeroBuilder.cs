using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

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

    //Mvc.AddNewtonsoftJson(x =>
    //{
    //  x.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
    //  x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    //  x.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
    //});

    // create startup options
    StartupOptions = new ZeroStartupOptions(Mvc);
    StartupOptions.AssemblyDiscoveryRules.Add(new ZeroAssemblyDiscoveryRule());
    setupAction?.Invoke(StartupOptions);

    services.AddControllers();

    // adds and discovers additional and built-in assemblies
    new AssemblyDiscovery(Mvc).Execute(StartupOptions.AssemblyDiscoveryRules);

    AddModule<ConfigurationModule>();
    AddModule<ContextModule>();
    AddModule<ArchitectureModule>();
    AddModule<CommunicationModule>();
    AddModule<IdentityModule>();
    AddModule<ApplicationModule>();
    AddModule<PersistenceModule>();
    AddModule<StoresModule>();
    AddModule<FileStorageModule>();
    AddModule<MapperModule>();
    AddModule<LocalizationModule>();
    AddModule<RenderingModule>();

    AddModule<RoutingModule>();
    AddModule<MailsModule>();
    AddModule<MediaModule>();
    AddModule<PagesModule>();
    AddModule<SpacesModule>();

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


  public ZeroBuilder AddModule<T>() where T : class, IZeroModule, new()
  {
    ZeroModuleCollection.AddModule<T>(Services, Configuration);
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
