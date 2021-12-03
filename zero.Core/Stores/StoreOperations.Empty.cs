namespace zero.Stores;

public partial class StoreOperations : IStoreOperations
{
  /// <inheritdoc />
  public virtual Task<T> Empty<T>(string flavorAlias = null) where T : ZeroIdEntity, ISupportsFlavors, new() => Empty<T, T>(flavorAlias);


  /// <inheritdoc />
  public virtual Task<TFlavor> Empty<T, TFlavor>(string flavorAlias = null)
    where T : ZeroIdEntity, ISupportsFlavors, new()
    where TFlavor : T, new()
  {
    // throw if this entity is not allowed to be created without a flavor
    if (flavorAlias.IsNullOrEmpty() && !Flavors.CanUseWithoutFlavors<T>())
    {
      string defaultFlavor = Flavors.DefaultFlavorFor<T>();

      if (defaultFlavor.IsNullOrEmpty())
      {
        throw new ArgumentException("Can not create instance of an entity which is configured to to be only used as a flavor", nameof(flavorAlias));
      }

      flavorAlias = defaultFlavor;
    }

    // return default instance if no flavor is required
    if (flavorAlias.IsNullOrEmpty())
    {
      return Task.FromResult<TFlavor>(new());
    }

    // try to load and construct a specific flavor
    FlavorConfig config = Flavors.Get<T>(flavorAlias);
    TFlavor result = config?.Construct(config) as TFlavor;

    if (result == null)
    {
      return Task.FromResult<TFlavor>(default);
    }

    result.Flavor = flavorAlias;
    return Task.FromResult(result);
  }
}