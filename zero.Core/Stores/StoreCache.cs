using System.Collections.Concurrent;

namespace zero.Stores;

public class StoreCache : IStoreCache
{
  protected ConcurrentDictionary<string, object> _cache = new();


  public StoreCache()
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

  //public IStoreCache For(CachableEntityStoreOptions options)
  //{
  //  return this;
  //}
}


public interface IStoreCache
{
  //IStoreCache For(CachableEntityStoreOptions options);

  bool TryGetValue<T>(string id, out T model);
}