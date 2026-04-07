using Microsoft.Extensions.DependencyModel;

namespace Finch.Assemblies;

public class FinchAssemblyDiscoveryRule : IAssemblyDiscoveryRule
{
  const string FinchPrefix = "Finch.";

  /// <inheritdoc />
  public bool IsValid(RuntimeLibrary library, AssemblyDiscoveryContext context)
  {
    StringComparison casing = StringComparison.OrdinalIgnoreCase;
    // TODO we need to auto-add assemblies and discover their types which have implementations of IFinchPlugin
    return library.Name.StartsWith(FinchPrefix, casing) || (context.HasEntryAssembly && library.Name.Contains(context.EntryAssemblyName, casing));
  }
}