using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace zero.Architecture;

internal class ZeroModuleCollection
{
  static ConcurrentDictionary<Type, IZeroModule> _modules = new();


  /// <summary>
  /// Get all registered modules
  /// </summary>
  public static IEnumerable<IZeroModule> GetAll() => _modules.Values;


  /// <summary>
  /// Adds a zero module
  /// </summary>
  public static void AddModule<T>(IServiceCollection services, IConfiguration configuration) where T : class, IZeroModule, new()
  {
    AddModule(services, configuration, new T());
  }


  /// <summary>
  /// Adds a zero module
  /// </summary>
  public static void AddModule<T>(IServiceCollection services, IConfiguration configuration, T moduleInstance) where T : IZeroModule
  {
    AddModule(typeof(T), services, configuration, moduleInstance);
  }


  /// <summary>
  /// Adds a zero module
  /// </summary>
  public static void AddModule(Type moduleType, IServiceCollection services, IConfiguration configuration, IZeroModule moduleInstance)
  {
    if (_modules.ContainsKey(moduleType))
    {
      return;
    }

    _modules.TryAdd(moduleType, moduleInstance);

    moduleInstance.ConfigureServices(services, configuration);
  }
}