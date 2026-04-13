using Microsoft.Extensions.DependencyInjection;

namespace Finch.Configuration;

public class FinchStartupOptions(IMvcBuilder mvc) : IFinchStartupOptions
{
  public IList<IAssemblyDiscoveryRule> AssemblyDiscoveryRules { get; } = new List<IAssemblyDiscoveryRule>();

  public IMvcBuilder Mvc { get; } = mvc;
}

public interface IFinchStartupOptions
{
  IList<IAssemblyDiscoveryRule> AssemblyDiscoveryRules { get; }

  IMvcBuilder Mvc { get; }
}