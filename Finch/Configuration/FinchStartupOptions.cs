using Microsoft.Extensions.DependencyInjection;

namespace Finch.Configuration;

public class FinchStartupOptions : IFinchStartupOptions
{
  public IList<IAssemblyDiscoveryRule> AssemblyDiscoveryRules { get; } = new List<IAssemblyDiscoveryRule>();

  public IMvcBuilder Mvc { get; }


  public FinchStartupOptions(IMvcBuilder mvc)
  {
    Mvc = mvc;
  }
}

public interface IFinchStartupOptions
{
  IList<IAssemblyDiscoveryRule> AssemblyDiscoveryRules { get; }

  IMvcBuilder Mvc { get; }
}