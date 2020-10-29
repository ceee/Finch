using Microsoft.Extensions.DependencyModel;
using System;
using zero.Core.Assemblies;

namespace zero.Web.Defaults
{
  public class ZeroAssemblyDiscoveryRule : IAssemblyDiscoveryRule
  {
    const string ZERO_PREFIX = "zero.";

    /// <inheritdoc />
    public bool IsValid(RuntimeLibrary library, AssemblyDiscoveryContext context)
    {
      StringComparison casing = StringComparison.OrdinalIgnoreCase;
      // TODO we need to auto-add assemblies and discover their types which have implementations of IZeroPlugin
      return library.Name.StartsWith(ZERO_PREFIX, casing) || (context.HasEntryAssembly && library.Name.Contains(context.EntryAssemblyName, casing));
    }
  }
}
