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
    return Task.FromResult(Flavors.Construct<T, TFlavor>(flavorAlias));
  }
}