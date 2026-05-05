using Microsoft.Extensions.DependencyInjection;

namespace Mixtape.Configuration;

public class MixtapeStartupOptions(IMvcBuilder mvc) : IMixtapeStartupOptions
{
  public IList<IAssemblyDiscoveryRule> AssemblyDiscoveryRules { get; } = new List<IAssemblyDiscoveryRule>();

  public IMvcBuilder Mvc { get; } = mvc;
}

public interface IMixtapeStartupOptions
{
  IList<IAssemblyDiscoveryRule> AssemblyDiscoveryRules { get; }

  IMvcBuilder Mvc { get; }
}