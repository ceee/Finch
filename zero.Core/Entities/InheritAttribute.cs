using System;

namespace zero.Core.Entities
{
  public class OverwriteAttribute : Attribute
  {
  }


  public class ReferenceAttribute : Attribute
  {
    public Type Type { get; set; }

    public ReferenceAttribute(Type type)
    {
      Type = type;
    }
  }
}