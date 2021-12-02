namespace zero.Stores;

public partial class StoreOperations : IStoreOperations
{
  /// <inheritdoc />
  public virtual Task<T> Empty<T>(string flavor = null) where T : ZeroIdEntity, ISupportsFlavors, new() => Empty<T, T>(flavor);


  /// <inheritdoc />
  public virtual Task<TFlavor> Empty<T, TFlavor>(string flavor)
    where T : ZeroIdEntity, ISupportsFlavors, new()
    where TFlavor : T, new()
  {
    // throw if this entity is not allowed to be created without a flavor
    if (flavor.IsNullOrEmpty() && !Flavors.CanUseWithoutFlavors<T>())
    {
      throw new ArgumentException("Can not create instance of an entity which is configured to to be only used as a flavor", nameof(flavor));
    }

    // return default instance if no flavor is required
    if (flavor.IsNullOrEmpty())
    {
      return Task.FromResult<TFlavor>(new());
    }

    // try to load and construct a specific flavor
    FlavorConfig config = Flavors.Get<T>(flavor);
    TFlavor result = config?.Construct(config) as TFlavor;

    if (result == null)
    {
      return Task.FromResult<TFlavor>(default);
    }

    result.Flavor = flavor;
    return Task.FromResult(result);
  }
}