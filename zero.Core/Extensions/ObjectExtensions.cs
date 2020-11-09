using Newtonsoft.Json;
using System;
using zero.Core.Utils;

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


    public static T Clone<T>(this T obj)
    {
      Type type = obj.GetType();
      return (T)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(obj, new RefJsonConverter()), type, new RefJsonConverter());
    }

    public static T CloneLax<T>(this object obj)
    {
      return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj, new RefJsonConverter()), new RefJsonConverter());
    }
  }
}
