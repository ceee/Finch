using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Architecture;

internal class ZeroPluginInitializer
{
  /// <summary>
  /// Adds a zero plugin
  /// </summary>
  public static void AddPlugin<T>(IServiceCollection services, IConfiguration configuration) where T : class, IZeroPlugin, new()
  {
    AssemblyDiscovery.Current.AddAssembly(typeof(T).Assembly);
    services.AddSingleton<IZeroPlugin, T>();
    AddPluginServices<T>(services, configuration);
  }


  /// <summary>
  /// Adds a zero plugin
  /// </summary>
  public static void AddPlugin<T>(IServiceCollection services, IConfiguration configuration, Func<IServiceProvider, T> implementationFactory) where T : class, IZeroPlugin, new()
  {
    AssemblyDiscovery.Current.AddAssembly(typeof(T).Assembly);
    services.AddSingleton<IZeroPlugin, T>(implementationFactory);
    AddPluginServices<T>(services, configuration);
  }


  /// <summary>
  /// Creates a temporary instance of the plugin to add additional services
  /// </summary>
  /// <typeparam name="T"></typeparam>
  static void AddPluginServices<T>(IServiceCollection services, IConfiguration configuration) where T : class, IZeroPlugin, new()
  {
    try
    {
      T plugin = new();

      plugin.ConfigureServices(services, configuration);
      services.Configure<ZeroOptions>(opts => plugin.Configure(opts));
    }
    catch
    {
      throw new Exception($"Plugin \"{nameof(T)}\" needs an additional parameterless constructor as ConfigureServices() is called before the DI container is built");
    }
  }
}