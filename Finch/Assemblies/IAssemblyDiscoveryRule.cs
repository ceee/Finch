using Microsoft.Extensions.DependencyModel;

namespace Finch.Assemblies;

public interface IAssemblyDiscoveryRule
{
  /// <summary>
  /// Returns true if the specified runtime library should be added to
  /// the ApplicationPartManager; otherwise false.
  /// </summary>
  bool IsValid(RuntimeLibrary library, AssemblyDiscoveryContext context);
}