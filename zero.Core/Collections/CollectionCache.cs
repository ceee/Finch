using System.Collections.Concurrent;

namespace zero.Collections;

public class CollectionCache : ICollectionCache
{
  protected ConcurrentDictionary<string, object> _cache = new();


  public CollectionCache()
  {

  }

  public bool TryGetValue<T>(string id, out T model)
  {
    if (_cache.TryGetValue(id, out object modelObj))
    {
      model = (T)modelObj;
      return true;
    }

    model = default;
    return false;
  }

  public ICollectionCache For(CachableEntityCollectionOptions options)
  {
    return this;
  }
}


public interface ICollectionCache
{
  ICollectionCache For(CachableEntityCollectionOptions options);

  bool TryGetValue<T>(string id, out T model);
}