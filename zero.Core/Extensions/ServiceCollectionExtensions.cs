using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using zero.Core.Assemblies;

namespace zero.Core.Extensions
{
  public static class ServiceCollectionExtensions
  {
    /// <summary>
    /// Adds all found implementations based on the service type and assembly discovery rules
    /// </summary>
    public static void AddAll<TService>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient) => services.AddAll(typeof(TService), lifetime);


    /// <summary>
    /// Adds all found implementations based on the service type and assembly discovery rules
    /// </summary>
    public static void AddAll(this IServiceCollection services, Type serviceType, ServiceLifetime lifetime = ServiceLifetime.Transient)
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
          IEnumerable<Type> genericTypes = type.GetInterfaces().Select(x => x.GetTypeInfo()).Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == serviceType);
          return genericTypes.Select(x => (x, type));
        });

        foreach ((Type, Type) match in matches)
        {
          services.Add(new ServiceDescriptor(match.Item1, match.Item2, lifetime));
        }
      }
      // add implementations with specific service types
      else
      {
        foreach (Type type in AssemblyDiscovery.Current.GetTypes(serviceType))
        {
          services.Add(new ServiceDescriptor(serviceType, type, lifetime));
        }
      }
    }


    /// <summary>
    /// Adds or overrides an implementation
    /// </summary>
    public static void Replace<TService, TImplementation>(this IServiceCollection services) 
      where TService : class
      where TImplementation : class, TService
    {
      services.Remove<TService>(out ServiceLifetime lifetime);
      services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime));
    }


    /// <summary>
    /// Adds or overrides an implementation
    /// </summary>
    public static void Replace<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
      where TService : class
      where TImplementation : class, TService
    {
      services.Remove<TService>(out ServiceLifetime lifetime);
      services.Add(new ServiceDescriptor(typeof(TService), implementationFactory, lifetime));
    }


    /// <summary>
    /// Removes all instances of a registered service
    /// </summary>
    public static bool Remove<TService>(this IServiceCollection services) => services.Remove<TService>(out ServiceLifetime lifetime);


    /// <summary>
    /// Removes all instances of a registered service
    /// </summary>
    public static bool Remove<TService>(this IServiceCollection services, out ServiceLifetime lifetime)
    {
      Type serviceType = typeof(TService);
      IEnumerable<ServiceDescriptor> descriptors = services.Where(x => x.ServiceType == serviceType);

      if (descriptors.Any())
      {
        lifetime = descriptors.First().Lifetime;

        foreach (ServiceDescriptor descriptor in descriptors)
        {
          services.Remove(descriptor);
        }
      }
      else
      {
        lifetime = ServiceLifetime.Transient;
      }

      return descriptors.Any();
    }
  }
}
