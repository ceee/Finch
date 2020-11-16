using System;

namespace zero.Core.Attributes
{
  /// <summary>
  /// This attribute will allow the usage of custom collection names for Raven collections
  /// </summary>
  [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false)]
  public class CollectionAttribute : Attribute
  {
    public string Name { get; set; }

    public bool LongId { get; set; } = false;

    public CollectionAttribute(string name, bool longId = false)
    {
      Name = name;
      LongId = longId;
    }
  }
}
