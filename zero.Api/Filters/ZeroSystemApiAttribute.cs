namespace zero.Api.Filters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
internal class ZeroSystemApiAttribute : Attribute
{
  public ZeroSystemApiAttribute() : base()
  {
    
  }
}