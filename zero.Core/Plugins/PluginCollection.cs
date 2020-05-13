using Microsoft.Extensions.DependencyInjection;
using System;

namespace zero.Core.Plugins
{
  public class PluginCollection
  {
    IServiceCollection Services;

    IZeroPluginBuilder Builder;


    public PluginCollection(IServiceCollection services, IZeroPluginBuilder builder)
    {
      Services = services;
      Builder = builder;
    }


    /// <summary>
    /// Adds a zero plugin
    /// </summary>
    public void Add<T>() where T : class, IZeroPlugin, new()
    {
      Services.AddScoped<IZeroPlugin, T>();
      AddPluginServices<T>();
    }


    /// <summary>
    /// Adds a zero plugin
    /// </summary>
    public void Add<T>(Func<IServiceProvider, T> implementationFactory) where T : class, IZeroPlugin, new()
    {
      Services.AddScoped<IZeroPlugin, T>(implementationFactory);
      AddPluginServices<T>();
    }


    /// <summary>
    /// Creates a temporary instance of the plugin to add additional services
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void AddPluginServices<T>() where T : class, IZeroPlugin, new()
    {
      try
      {
        new T().Configure(Services, Builder);
      }
      catch
      {
        throw new Exception($"Plugin \"{nameof(T)}\" needs an additional parameterless constructor as ConfigureServices() is called before the DI container is built");
      }
    }
  }
}
