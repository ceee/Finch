namespace zero.Utils;

/// <summary>
/// Automatically generate ID with the specified length and insert it into this property on entity save
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class GenerateIdAttribute : Attribute
{
  public int? Length = null;

  public GenerateIdAttribute(int length)
  {
    Length = length;
  }

  public GenerateIdAttribute() { }
}
