using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace zero.Extensions;

public static class ServiceCollectionExtensions
{
  /// <summary>
  /// Adds all found implementations based on the service type and assembly discovery rules
  /// </summary>
  public static void AddAll<TService>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient, bool asSelf = false) => services.AddAll(typeof(TService), lifetime, asSelf);


  /// <summary>
  /// Adds all found implementations based on the service type and assembly discovery rules
  /// </summary>
  public static void AddAll(this IServiceCollection services, Type serviceType, ServiceLifetime lifetime = ServiceLifetime.Transient, bool asSelf = false)
  {
    if (AssemblyDiscovery.Current == null)
    {
      throw new Exception("services.AddAll() can only be run after mvcBuilder.AddZero()");
    }

    // add implementations with generic service types
    if (serviceType.GetTypeInfo().IsGenericTypeDefinition)
    {
      IEnumerable<(Type, TypeInfo)> matches = AssemblyDiscovery.Current.GetAllClassTypes().SelectMany(type =>
      {
        var interfaces = type.GetInterfaces().Select(x => x.GetTypeInfo());
        IEnumerable<Type> genericTypes = interfaces.Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == serviceType);
        if (genericTypes.Any())
        {
          var genericTypeDefinitions = genericTypes.First();
        }
        return genericTypes.Select(x => (x, type));
      });

      foreach ((Type, Type) match in matches)
      {
        services.Add(new ServiceDescriptor(asSelf ? match.Item2 : match.Item1, match.Item2, lifetime));
      }
    }
    // add implementations with specific service types
    else
    {
      foreach (Type type in AssemblyDiscovery.Current.GetTypes(serviceType))
      {
        services.Add(new ServiceDescriptor(asSelf ? type : serviceType, type, lifetime));
      }
    }
  }


  /// <summary>
  /// Adds or overrides an implementation
  /// </summary>
  public static void Replace<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient) 
    where TService : class
    where TImplementation : class, TService
  {
    services.RemoveAll<TService>();
    services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime));
  }


  /// <summary>
  /// Adds or overrides an implementation
  /// </summary>
  public static void Replace<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory, ServiceLifetime lifetime = ServiceLifetime.Transient)
    where TService : class
    where TImplementation : class, TService
  {
    services.RemoveAll<TService>();
    services.Add(new ServiceDescriptor(typeof(TService), implementationFactory, lifetime));
  }
}
