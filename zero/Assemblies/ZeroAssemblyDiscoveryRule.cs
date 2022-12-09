using Microsoft.Extensions.DependencyModel;

namespace zero.Assemblies;

public class ZeroAssemblyDiscoveryRule : IAssemblyDiscoveryRule
{
  const string ZeroPrefix = "zero.";

  /// <inheritdoc />
  public bool IsValid(RuntimeLibrary library, AssemblyDiscoveryContext context)
  {
    StringComparison casing = StringComparison.OrdinalIgnoreCase;
    // TODO we need to auto-add assemblies and discover their types which have implementations of IZeroPlugin
    return library.Name.StartsWith(ZeroPrefix, casing) || (context.HasEntryAssembly && library.Name.Contains(context.EntryAssemblyName, casing));
  }
}