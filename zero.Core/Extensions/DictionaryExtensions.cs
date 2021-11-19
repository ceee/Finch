using System;
using System.Collections.Generic;

namespace zero;

public static class DictionaryExtensions
{
  public static bool TryGetValue<T>(this Dictionary<string, object> model, string key, out T value)
  {
    if (!model.TryGetValue(key, out object valueObj) || !(valueObj is T))
    {
      value = default;
      return false;
    }

    value = (T)valueObj;
    return true;
  }


  public static T GetValueOrDefault<T>(this Dictionary<string, object> model, string key)
  {
    object value = model.GetValueOrDefault(key);
    return value == default || !(value is T) ? default : (T)value; 
  }


  public static Dictionary<TKey, TElement> ToDistinctDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
  {
    Dictionary<TKey, TElement> result = new();

    foreach (TSource sourceElement in source)
    {
      result.TryAdd(keySelector(sourceElement), elementSelector(sourceElement));
    }

    return result;
  }
}
