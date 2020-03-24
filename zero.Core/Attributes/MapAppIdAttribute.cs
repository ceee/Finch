using System;

namespace zero.Core.Attributes
{
  /// <summary>
  /// This attribute automatically inserts the current application id into this property (has to be a string) on entity save
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class MapAppIdAttribute : Attribute
  {
    public MapAppIdAttribute() { }
  }
}
