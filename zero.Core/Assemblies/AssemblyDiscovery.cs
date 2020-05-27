using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace zero.Core.Assemblies
{
  public class AssemblyDiscovery : IAssemblyDiscovery
  {
    AssemblyDiscoveryContext Context;

    IMvcBuilder Mvc;


    public AssemblyDiscovery(IMvcBuilder mvc)
    {
      Mvc = mvc;
      Context = new AssemblyDiscoveryContext();
    }


    /// <inheritdoc />
    public void Execute(IEnumerable<IAssemblyDiscoveryRule> rules)
    {
      List<Assembly> assemblies = new List<Assembly>();
      DependencyContext dependencyContext = DependencyContext.Load(Context.EntryAssembly);

      if (dependencyContext == null)
      {
        return;
      }

      string[] existingLibs = Mvc.PartManager.ApplicationParts.OfType<AssemblyPart>().Select(p => p.Name).ToArray();

      IEnumerable<RuntimeLibrary> libraries = dependencyContext.RuntimeLibraries.Where(lib => !existingLibs.Contains(lib.Name, StringComparer.OrdinalIgnoreCase));

      foreach (RuntimeLibrary library in libraries)
      {
        if (rules.Any(rule => rule.IsValid(library, Context)))
        {
          IEnumerable<Assembly> libraryAssemblies = library.GetDefaultAssemblyNames(dependencyContext).Select(name => Assembly.Load(name));

          foreach (Assembly assembly in libraryAssemblies)
          {
            AddAssembly(assembly);
          }
        }
      }
    }


    /// <inheritdoc />
    public void AddAssembly(Assembly assembly)
    {
      Mvc.AddApplicationPart(assembly);
    }


    /// <inheritdoc />
    public IEnumerable<TypeInfo> GetTypes<TService>() => GetTypes(typeof(TService));


    /// <inheritdoc />
    public IEnumerable<TypeInfo> GetTypes(Type serviceType)
    {
      return GetConcreteTypes().Where(t => serviceType.GetTypeInfo().IsAssignableFrom(t) && t.AsType() != serviceType);
    }


    /// <summary>
    /// Get all registered types
    /// </summary>
    public IEnumerable<TypeInfo> GetConcreteTypes() => GetTypes().Where(t => t.IsClass  && !t.IsAbstract && !t.ContainsGenericParameters);


    /// <summary>
    /// Get all registered assemblies
    /// </summary>
    public IEnumerable<Assembly> GetAssemblies() => Mvc.PartManager.ApplicationParts.OfType<AssemblyPart>().Select(p => p.Assembly);


    /// <summary>
    /// Get all registered types
    /// </summary>
    public IEnumerable<TypeInfo> GetTypes() => Mvc.PartManager.ApplicationParts.OfType<AssemblyPart>().SelectMany(p => p.Types);
  }


  public interface IAssemblyDiscovery
  {
    /// <summary>
    /// Discovers runtime assemblies based on the given rules
    /// </summary>
    void Execute(IEnumerable<IAssemblyDiscoveryRule> rules);

    /// <summary>
    /// Manually add an assembly
    /// </summary>
    void AddAssembly(Assembly assembly);

    /// <summary>
    /// Get all discovered types which implement a certain service
    /// </summary>
    IEnumerable<TypeInfo> GetTypes<TService>();

    /// <summary>
    /// Get all discovered types which implement a certain service
    /// </summary>
    IEnumerable<TypeInfo> GetTypes(Type serviceType);

    /// <summary>
    /// Get all registered types
    /// </summary>
    IEnumerable<TypeInfo> GetTypes();

    /// <summary>
    /// Get all registered types
    /// </summary>
    IEnumerable<TypeInfo> GetConcreteTypes();

    /// <summary>
    /// Get all registered assemblies
    /// </summary>
    IEnumerable<Assembly> GetAssemblies();
  }
}
