using System;

namespace zero;

/// <summary>
/// This attribute will allow the usage of custom collection names for Raven collections
/// </summary>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false)]
public class RavenCollectionAttribute : Attribute
{
  public string Name { get; set; }

  public RavenCollectionAttribute(string name)
  {
    Name = name;
  }
}
