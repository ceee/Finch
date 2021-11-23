namespace zero.Stores;

public record CachableEntityStoreOptions(bool CacheIndividual, bool CacheAll);


public abstract class CachableEntityStore<T> : EntityStore<T> where T : ZeroIdEntity, new()
{
  protected CachableEntityStoreOptions CacheOptions { get; set; }

  protected IStoreCache Cache { get; set; }


  public CachableEntityStore(IStoreContext collectionContext, IStoreCache collectionCache) : base(collectionContext)
  {
    CacheOptions = new(CacheIndividual: true, CacheAll: false); // TODO when props update we need to update Cache.For()
    Cache = collectionCache.For(CacheOptions);
  }


  /// <inheritdoc />
  public override async Task<T> Load(string id, string changeVector = null)
  {
    if (changeVector.IsNullOrEmpty() && Cache.TryGetValue(id, out T model))
    {
      return model;
    }

    return await base.Load(id, changeVector);
  }


  /// <inheritdoc />
  public override Task<Dictionary<string, T>> Load(IEnumerable<string> ids)
  {
    return base.Load(ids);
  }
}