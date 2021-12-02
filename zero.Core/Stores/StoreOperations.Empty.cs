namespace zero.Stores;

public partial class StoreOperations : IStoreOperations
{
  /// <summary>
  /// Get new instance of an entity
  /// </summary>
  public virtual Task<T> Empty<T>() where T : ZeroIdEntity, new()
  {
    return Task.FromResult(new T());
  }


  /// <inheritdoc />
  public virtual Task<T> Empty<T>(string flavor) where T : ZeroIdEntity, ISupportsFlavors, new()
  {
    FlavorConfig config = Flavors.Get<T>(flavor);
    T result = config?.Construct(config) as T;

    if (result == null)
    {
      return Task.FromResult<T>(default);
    }

    result.Flavor = flavor;
    return Task.FromResult(result);
  }


  /// <inheritdoc />
  public virtual Task<TFlavor> Empty<T, TFlavor>(string flavor)
    where T : ZeroIdEntity, ISupportsFlavors, new()
    where TFlavor : T
  {
    FlavorConfig config = Flavors.Get<T, TFlavor>();
    TFlavor result = config?.Construct(config) as TFlavor;

    if (result == null)
    {
      return Task.FromResult<TFlavor>(default);
    }

    result.Flavor = flavor;
    return Task.FromResult(result);
  }
}