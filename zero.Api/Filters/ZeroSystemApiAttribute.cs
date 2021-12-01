namespace zero.Api.Filters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ZeroSystemApiAttribute : Attribute
{
  public ZeroSystemApiAttribute() : base()
  {
    
  }
}