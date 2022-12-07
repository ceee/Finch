using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace zero.Architecture;

public class ZeroModuleCollection : ZeroModule
{
  ConcurrentDictionary<Type, IZeroModule> _modules = new();


  /// <summary>
  /// Get all registered modules
  /// </summary>
  public IEnumerable<IZeroModule> GetAll() => _modules.Values;


  /// <summary>
  /// Adds a zero module
  /// </summary>
  public void Add<T>() where T : class, IZeroModule, new()
  {
    Add(new T());
  }


  /// <summary>
  /// Adds a zero module
  /// </summary>
  public void Add<T>(T moduleInstance) where T : IZeroModule
  {
    Add(typeof(T), moduleInstance);
  }


  /// <summary>
  /// Adds a zero module
  /// </summary>
  public void Add(Type moduleType, IZeroModule moduleInstance)
  {
    if (_modules.ContainsKey(moduleType))
    {
      return;
    }

    _modules.TryAdd(moduleType, moduleInstance);
  }


  /// <inheritdoc />
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    foreach (var module in _modules.OrderBy(x => x.Value.Order))
    {
      module.Value.ConfigureServices(services, configuration);
    }
  }


  /// <inheritdoc />
  public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
  {
    foreach (var module in _modules.OrderBy(x => x.Value.ConfigureOrder))
    {
      module.Value.Configure(app, routes, serviceProvider);
    }
  }
}