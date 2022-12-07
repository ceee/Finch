using System.Reflection;

namespace zero.Architecture;

public class AssemblyDiscoveryContext
{
  public Assembly EntryAssembly { get; private set; }

  public string EntryAssemblyName { get; private set; }

  public bool HasEntryAssembly => EntryAssembly != null;


  public AssemblyDiscoveryContext()
  {
    EntryAssembly = Assembly.GetEntryAssembly();
    EntryAssemblyName = EntryAssembly?.GetName().Name;
  }


  public AssemblyDiscoveryContext(Assembly entryAssembly)
  {
    EntryAssembly = entryAssembly;
    EntryAssemblyName = entryAssembly.GetName().Name;
  }
}
