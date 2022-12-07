//using Newtonsoft.Json;

//namespace zero.Extensions;

//[Obsolete("we don't want this for every object (use a Utils class instead)")]
//public static class ObjectExtensions
//{
//  public static bool Is<T>(this Type type)
//  {
//    return type.IsAssignableFrom(typeof(T));
//  }


//  public static bool Is<T>(this object obj)
//  {
//    return obj.GetType().IsAssignableFrom(typeof(T));
//  }


//  public static T Clone<T>(this T obj)
//  {
//    Type type = obj.GetType();
//    return (T)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(obj), type);
//  }

//  public static T CloneLax<T>(this object obj)
//  {
//    return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
//  }
//}
