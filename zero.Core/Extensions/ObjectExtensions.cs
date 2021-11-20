using Newtonsoft.Json;

namespace zero.Extensions;

[Obsolete("we don't want this for every object (use a Utils class instead)")]
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

  public static T AutoSetIds<T>(this T obj)
  {
    // find all Raven Ids
    List<ObjectTraverser.Result<GenerateIdAttribute>> ravenIds = ObjectTraverser.FindAttribute<GenerateIdAttribute>(obj);

    // set unset Raven Ids
    foreach (ObjectTraverser.Result<GenerateIdAttribute> item in ravenIds)
    {
      string id = item.Property.GetValue(item.Parent, null) as string;
      if (String.IsNullOrWhiteSpace(id))
      {
        item.Property.SetValue(item.Parent, item.Item.Length.HasValue ? IdGenerator.Create(item.Item.Length.Value) : IdGenerator.Create());
      }
    }

    return obj;
  }
}
