using FluentValidation;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Finch.Logging;
using Finch.Mails;
using Finch.Metadata;
using Finch.Mvc;
using Finch.Numbers;
using Finch.Routing;
using Finch.Security;

namespace Finch;

// TODO maybe use a middleware like Hangfire does: https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.AspNetCore/HangfireEndpointRouteBuilderExtensions.cs

public class FinchBuilder
{
  public virtual IServiceCollection Services { get; }

  public virtual IMvcBuilder Mvc { get; }

  internal static FinchModuleCollection Modules { get; } = new();

  readonly IConfiguration _configuration;
  readonly IFinchStartupOptions _startupOptions;


  public FinchBuilder(IServiceCollection services, IConfiguration configuration, Action<IFinchStartupOptions> setupAction)
  {
    Services = services;
    Mvc = services.AddMvc();
    _configuration = configuration;

    // create startup options
    _startupOptions = new FinchStartupOptions(Mvc);
    _startupOptions.AssemblyDiscoveryRules.Add(new FinchAssemblyDiscoveryRule());
    setupAction?.Invoke(_startupOptions);

    //string appName = configuration.GetValue<string>("Finch:AppName").Or("finch-app");
    //services.AddDataProtection();.PersistKeysToRegistry()

    services.AddControllers();
    services.AddOutputCache();
    Mvc = services.AddRazorPages();

    Mvc.AddDataAnnotationsLocalization();

    services.Configure<AntiforgeryOptions>(opts => opts.Cookie.Name = "finch.antiforgery");


    // adds and discovers additional and built-in assemblies
    new AssemblyDiscovery(Mvc).Execute(_startupOptions.AssemblyDiscoveryRules);

    Modules.Add<FinchLoggingModule>();
    Modules.Add<FinchCommunicationModule>();
    Modules.Add<FinchMvcModule>();
    Modules.Add<FinchConfigurationModule>();
    Modules.Add<FinchValidationModule>();
    Modules.Add<FinchContextModule>();
    Modules.Add<FinchFileStorageModule>();
    //Modules.Add<FinchIdentityModule>();
    Modules.Add<FinchLocalizationModule>();
    Modules.Add<FinchMailModule>();
    //Modules.Add<FinchMapperModule>();
    Modules.Add<FinchMediaModule>();
    //Modules.Add<FinchPageModule>();
    Modules.Add<FinchRenderingModule>();
    Modules.Add<FinchNumberModule>();
    Modules.Add<FinchRoutingModule>();
    Modules.Add<FinchMetadataModule>();
    Modules.Add<FinchSecurityModule>();

    Modules.ConfigureServices(services, configuration);

    // configure FluentValidation
    ValidatorOptions.Global.PropertyNameResolver = ValidatorCamelCasePropertyResolver.ResolvePropertyName;
  }


  /// <summary>
  /// Use specified options
  /// </summary>
  public FinchBuilder WithOptions(Action<FinchOptions> configureOptions)
  {
    Services.Configure(configureOptions);
    return this;
  }


  public FinchBuilder AddModule(IFinchModule module)
  {
    module.ConfigureServices(Services, _configuration);
    Modules.Add(module);
    return this;
  }
  
  
  public FinchBuilder AddModule<T>() where T : class, IFinchModule, new()
  {
    T module = new();
    module.ConfigureServices(Services, _configuration);
    Modules.Add(module);
    return this;
  }
}
