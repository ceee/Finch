using Microsoft.Extensions.DependencyInjection;

namespace zero.Configuration;

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