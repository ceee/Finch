using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace zero;

public class ObjectCopycat
{
  const string SEPARATOR = ".";

  static Type STRING_TYPE = typeof(string);

  static ConcurrentDictionary<string, IEnumerable<PropertyInfo>> PublicPropertiesPerType { get; set; } = new();



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
        property.SetValue(target, propertyValue, null);
      }
      else if (property.PropertyType.IsClass)
      {
        string[] nestedExceptions = exceptions.Where(x => x.StartsWith(propertyNameWithPrefix, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        property.SetValue(target, CopyPropertiesInternal(propertyValue, property.GetValue(target, null), propertyNameWithPrefix, nestedExceptions), null);
      }
    }

    return target;
  }


  private static string CombineKey(string sep, string key1, string key2)
  {
    if (String.IsNullOrEmpty(key1))
    {
      return key2;
    }
    return String.Join(sep, key1, key2);
  }
}
