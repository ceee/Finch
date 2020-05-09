using System;

namespace zero.Core.Extensions
{
  public static class ObjectExtensions
  {
    public static bool Is<T>(this Type type)
    {
      return type.IsAssignableFrom(typeof(T));
    }


    public static bool Is<T>(this object obj)
    {
      return obj.GetType().IsAssignableFrom(typeof(T));
    }
  }
}
