using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using zero.Core.Assemblies;

namespace zero.Core.Options
{
  public class ZeroStartupOptions : IZeroStartupOptions
  {
    public IList<IAssemblyDiscoveryRule> AssemblyDiscoveryRules { get; private set; } = new List<IAssemblyDiscoveryRule>();

    public IMvcBuilder Mvc { get; private set; }


    public ZeroStartupOptions(IMvcBuilder mvc)
    {
      Mvc = mvc;
    }
  }

  public interface IZeroStartupOptions
  {
    IList<IAssemblyDiscoveryRule> AssemblyDiscoveryRules { get; }

    IMvcBuilder Mvc { get; }
  }
}
