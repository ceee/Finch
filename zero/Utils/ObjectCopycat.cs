using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

namespace zero.Utils;

public class ObjectCopycat
{
  const string SEPARATOR = ".";

  static readonly Type STRING_TYPE = typeof(string);

  static ConcurrentDictionary<string, IEnumerable<PropertyInfo>> PublicPropertiesPerType { get; set; } = new();


  public static T Clone<T>(T obj)
  {
    Type type = obj.GetType();
    return (T)JsonSerializer.Deserialize(JsonSerializer.Serialize(obj), type);
  }


  public static T CloneSpecific<T>(T obj)
  {
    Type type = obj.GetType();
    return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(obj));
  }


  public static bool ContentEquals<T>(T obj1, T obj2)
  {
    return (obj1 == null && obj2 == null) || (obj1 != null && obj2 != null && JsonSerializer.Serialize(obj1) == JsonSerializer.Serialize(obj2));
  }


  public static T CopyProperties<T>(T source, T target, params string[] exceptions)
  {
    return CopyPropertiesInternal(source, target, null, exceptions);
  }


  static T CopyPropertiesInternal<T>(T source, T target, string keyPrefix = null, params string[] exceptions)
  {
    Type type = source.GetType();

    if (!PublicPropertiesPerType.TryGetValue(type.FullName, out IEnumerable<PropertyInfo> properties))
    {
      properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.CanWrite);
      PublicPropertiesPerType.TryAdd(type.FullName, properties);
    }

    foreach (PropertyInfo property in properties)
    {
      string propertyNameWithPrefix = CombineKey(SEPARATOR, keyPrefix, property.Name);

      if (exceptions.Contains(propertyNameWithPrefix, StringComparer.InvariantCultureIgnoreCase))
      {
        continue;
      }

      object propertyValue = property.GetValue(source, null);

      if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(STRING_TYPE) || propertyValue == null || propertyValue is IEnumerable)
      {
        target ??= (T)Activator.CreateInstance(type);
        property.SetValue(target, propertyValue, null);
      }
      else if (property.PropertyType.IsClass)
      {
        string[] nestedExceptions = exceptions.Where(x => x.StartsWith(propertyNameWithPrefix, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        target ??= (T)Activator.CreateInstance(type);
        property.SetValue(target, CopyPropertiesInternal(propertyValue, property.GetValue(target, null), propertyNameWithPrefix, nestedExceptions), null);
      }
    }

    return target;
  }


  private static string CombineKey(string sep, string key1, string key2)
  {
    if (string.IsNullOrEmpty(key1))
    {
      return key2;
    }
    return string.Join(sep, key1, key2);
  }
}
