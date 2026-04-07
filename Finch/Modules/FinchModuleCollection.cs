using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Finch.Modules;

public class FinchModuleCollection : FinchModule
{
  ConcurrentDictionary<Type, IFinchModule> _modules = new();


  /// <summary>
  /// Get all registered modules
  /// </summary>
  public IEnumerable<IFinchModule> GetAll() => _modules.Values;


  /// <summary>
  /// Adds a finch module
  /// </summary>
  public void Add<T>() where T : class, IFinchModule, new()
  {
    Add(new T());
  }


  /// <summary>
  /// Adds a finch module
  /// </summary>
  public void Add<T>(T moduleInstance) where T : IFinchModule
  {
    Add(typeof(T), moduleInstance);
  }


  /// <summary>
  /// Adds a finch module
  /// </summary>
  public void Add(Type moduleType, IFinchModule moduleInstance)
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