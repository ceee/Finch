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
using Raven.Client.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using zero.Core;
using zero.Core.Api;
using zero.Core.Assemblies;
using zero.Core.Collections;
using zero.Core.Cultures;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Handlers;
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


      Mvc.AddNewtonsoftJson(x =>
      {
        // TODO this shall only be configurated for backoffice controllers
        BackofficeJsonSerlializerSettings.Setup(x.SerializerSettings);
      });

      if (Environment.GetEnvironmentVariable("DOTNET_WATCH") == "1")
      {
        Mvc.AddRazorRuntimeCompilation();
      } 

      // configure FluentValidation
      ValidatorOptions.Global.PropertyNameResolver = ValidatorCamelCasePropertyResolver.ResolvePropertyName;


      // add default services
      Services.AddHttpContextAccessor();
      Services.AddScoped<IApplicationResolver, ApplicationResolver>();
      Services.AddScoped<ICultureResolver, CultureResolver>();
      Services.AddScoped<IZeroContext, ZeroContext>();

      Services.AddScoped<IBackofficeStore, BackofficeStore>();
      Services.AddScoped<BackofficeFilterAttribute>();
      Services.AddScoped<ModelStateValidationFilterAttribute>();

      //Services.AddScoped<ICollectionInterceptorHandler, CollectionInterceptorHandler>();

      Services.AddHttpContextAccessor();

      Services.AddTransient<IZeroVue, ZeroVue>();
      Services.AddScoped<IPaths>(factory => new Paths(factory.GetService<IWebHostEnvironment>(), true));

      Services.AddTransient<IHandlerHolder, HandlerHolder>();

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
