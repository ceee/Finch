using Microsoft.Extensions.DependencyModel;

namespace Mixtape.Assemblies;

public class MixtapeAssemblyDiscoveryRule : IAssemblyDiscoveryRule
{
  const string MixtapePrefix = "Mixtape.";

  /// <inheritdoc />
  public bool IsValid(RuntimeLibrary library, AssemblyDiscoveryContext context)
  {
    StringComparison casing = StringComparison.OrdinalIgnoreCase;
    // TODO we need to auto-add assemblies and discover their types which have implementations of IMixtapePlugin
    return library.Name.StartsWith(MixtapePrefix, casing) || (context.HasEntryAssembly && library.Name.Contains(context.EntryAssemblyName, casing));
  }
}