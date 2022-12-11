using FluentValidation;
using Microsoft.AspNetCore.Antiforgery;
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

  readonly IConfiguration _configuration;
  readonly IZeroStartupOptions _startupOptions;


  public ZeroBuilder(IServiceCollection services, IConfiguration configuration, Action<IZeroStartupOptions> setupAction)
  {
    Services = services;
    Mvc = services.AddMvc();
    _configuration = configuration;

    // create startup options
    _startupOptions = new ZeroStartupOptions(Mvc);
    _startupOptions.AssemblyDiscoveryRules.Add(new ZeroAssemblyDiscoveryRule());
    setupAction?.Invoke(_startupOptions);

    services.AddControllers();
    services.AddRazorPages();

    services.Configure<AntiforgeryOptions>(opts => opts.Cookie.Name = "zero.antiforgery");

    // adds and discovers additional and built-in assemblies
    new AssemblyDiscovery(Mvc).Execute(_startupOptions.AssemblyDiscoveryRules);
    
    Modules.Add<ZeroCommunicationModule>();
    Modules.Add<ZeroConfigurationModule>();
    Modules.Add<ZeroValidationModule>();
    Modules.Add<ZeroContextModule>();
    Modules.Add<ZeroFileStorageModule>();
    //Modules.Add<ZeroIdentityModule>();
    Modules.Add<ZeroLocalizationModule>();
    //Modules.Add<ZeroMailModule>();
    //Modules.Add<ZeroMapperModule>();
    Modules.Add<ZeroMediaModule>();
    //Modules.Add<ZeroPageModule>();
    Modules.Add<ZeroRenderingModule>();
    //Modules.Add<ZeroRoutingModule>();

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


  public ZeroBuilder AddModule(IZeroModule module)
  {
    module.ConfigureServices(Services, _configuration);
    Modules.Add(module);
    return this;
  }
  
  
  public ZeroBuilder AddModule<T>() where T : class, IZeroModule, new()
  {
    T module = new();
    module.ConfigureServices(Services, _configuration);
    Modules.Add(module);
    return this;
  }
}
