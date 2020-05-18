using System;

namespace zero.Core.Attributes
{
  /// <summary>
  /// This attribute will allow the usage of custom collection names for Raven collections
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class CollectionAttribute : Attribute
  {
    public string Name { get; set; }

    public CollectionAttribute(string name)
    {
      Name = name;
    }
  }
}
