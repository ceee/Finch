using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;
using System.Linq;
using System.Reflection;
using zero.Core.Api;

namespace zero.Core.Entities
{
  public static class ServiceCollectionExtensions
  {
    public static void AddZeroCoreServices(this IServiceCollection services)
    {
      //services.Scan(scan => scan.FromAssemblyOf<IApplication>()
      //  .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
      //  .AsImplementedInterfaces()
      //  //.As(typeof(IValidator<>))
      //  .WithScopedLifetime()
      //);

      //Assembly[] assemblies = new Assembly[1] { typeof(IApplication).Assembly };

      services.AddAllByInterfaceTransient(typeof(IValidator), typeof(IValidator<>), AppDomain.CurrentDomain.GetAssemblies());

      services.AddTransient<IApplication, Application>(); 
      services.AddTransient(typeof(IApplicationsApi<>), typeof(ApplicationsApi<>));
    }


    public static void AddAllByInterfaceTransient<TService, TImplementation>(this IServiceCollection services, Assembly[] assemblies)
    {
      services.AddAllByInterfaceTransient(typeof(TService), typeof(TImplementation), assemblies);
      //Type searchFor = typeof(TService);
      //string searchForName = nameof(TService); // + (isGeneric ? "`1" : String.Empty);

      //foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      //{
      //  foreach (Type type in assembly.GetExportedTypes())
      //  {
      //    if (searchFor.IsAssignableFrom(type) && type.IsClass)
      //    {
      //      Type service = type.GetInterfaces().FirstOrDefault(x => x.IsInterface && x.Name == searchForName);

      //      if (service != null)
      //      {
      //        services.AddTransient(service, type);
      //      }
      //    }
      //  }
      //}
    }


    public static void AddAllByInterfaceTransient(this IServiceCollection services, Type serviceType, Type implementingType, Assembly[] assemblies)
    {
      foreach (Assembly assembly in assemblies)
      {
        foreach (Type type in assembly.GetExportedTypes())
        {
          if (serviceType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract && !type.FullName.StartsWith("Fluent"))
          {
            Type service = type.GetInterfaces().FirstOrDefault(x => x.IsInterface && x.Name == implementingType.Name);

            if (service != null)
            {
              services.AddTransient(service, type);
            }
          }
        }
      }
    }
  }
}
