using FluentValidation;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mixtape.Logging;
using Mixtape.Mails;
using Mixtape.Metadata;
using Mixtape.Mvc;
using Mixtape.Numbers;
using Mixtape.Routing;
using Mixtape.Security;

namespace Mixtape;

// TODO maybe use a middleware like Hangfire does: https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.AspNetCore/HangfireEndpointRouteBuilderExtensions.cs

public class MixtapeBuilder
{
  public virtual IServiceCollection Services { get; }

  internal static MixtapeModuleCollection Modules { get; } = new();

  readonly IConfiguration _configuration;


  public MixtapeBuilder(IServiceCollection services, IConfiguration configuration, Action<IMixtapeStartupOptions> setupAction)
  {
    Services = services;
    _configuration = configuration;

    bool isWeb = services.Any(s => s.ServiceType.FullName == "Microsoft.AspNetCore.Hosting.IWebHostEnvironment");;

    if (isWeb)
    {
      IMvcBuilder mvcBuilder = services.AddMvc();
      // create startup options
      IMixtapeStartupOptions startupOptions = new MixtapeStartupOptions(mvcBuilder);
      startupOptions.AssemblyDiscoveryRules.Add(new MixtapeAssemblyDiscoveryRule());
      setupAction?.Invoke(startupOptions);

      services.AddResponseCaching();
      services.AddControllers();
      services.AddOutputCache();
      mvcBuilder = services.AddRazorPages();

      mvcBuilder.AddDataAnnotationsLocalization();

      services.Configure<AntiforgeryOptions>(opts => opts.Cookie.Name = "mixtape.antiforgery");

      // adds and discovers additional and built-in assemblies
      new AssemblyDiscovery(mvcBuilder).Execute(startupOptions.AssemblyDiscoveryRules);

      Modules.Add<MixtapeMvcModule>();
    }

    //string appName = configuration.GetValue<string>("Mixtape:AppName").Or("mixtape-app");
    //services.AddDataProtection();.PersistKeysToRegistry()

    Modules.Add<MixtapeLoggingModule>();
    Modules.Add<MixtapeCommunicationModule>();
    Modules.Add<MixtapeConfigurationModule>();
    Modules.Add<MixtapeValidationModule>();
    Modules.Add<MixtapeContextModule>();
    Modules.Add<MixtapeFileStorageModule>();
    Modules.Add<MixtapeLocalizationModule>();
    Modules.Add<MixtapeMailModule>();
    Modules.Add<MixtapeMediaModule>();
    Modules.Add<MixtapeRenderingModule>();
    Modules.Add<MixtapeNumberModule>();
    Modules.Add<MixtapeRoutingModule>();
    Modules.Add<MixtapeMetadataModule>();
    Modules.Add<MixtapeSecurityModule>();

    Modules.ConfigureServices(services, configuration);

    // configure FluentValidation
    ValidatorOptions.Global.PropertyNameResolver = ValidatorCamelCasePropertyResolver.ResolvePropertyName;
  }


  /// <summary>
  /// Use specified options
  /// </summary>
  public MixtapeBuilder WithOptions(Action<MixtapeOptions> configureOptions)
  {
    Services.Configure(configureOptions);
    return this;
  }


  public MixtapeBuilder AddModule(IMixtapeModule module)
  {
    module.ConfigureServices(Services, _configuration);
    Modules.Add(module);
    return this;
  }
  
  
  public MixtapeBuilder AddModule<T>() where T : class, IMixtapeModule, new()
  {
    T module = new();
    module.ConfigureServices(Services, _configuration);
    Modules.Add(module);
    return this;
  }
}
