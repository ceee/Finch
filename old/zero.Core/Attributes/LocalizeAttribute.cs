using System;

namespace zero.Core.Attributes
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false)]
  public class LocalizeAttribute : Attribute
  {
    public string Key;

    public LocalizeAttribute(string key)
    {
      Key = key;
    }
  }
}
