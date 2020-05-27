using System.Collections.Generic;
using zero.Core.Assemblies;

namespace zero.Core.Options
{
  public class ZeroStartupOptions : IZeroStartupOptions
  {
    public IList<IAssemblyDiscoveryRule> AssemblyDiscoveryRules { get; private set; } = new List<IAssemblyDiscoveryRule>();
  }

  public interface IZeroStartupOptions
  {
    IList<IAssemblyDiscoveryRule> AssemblyDiscoveryRules { get; }
  }
}
