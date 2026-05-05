namespace Mixtape.Models;

public class FlavorProviderOptions<TEntity> where TEntity : class, ISupportsFlavors, new()
{
  readonly Type _baseType;
  readonly FlavorOptions _options;
  readonly FlavorProvider _provider;

  internal FlavorProviderOptions(FlavorOptions options, FlavorProvider provider)
  {
    _baseType = typeof(TEntity);
    _options = options;
    _provider = provider;
  }

  public bool CanUseWithoutFlavors
  {
    get => _provider.CanUseWithoutFlavors;
    set => _provider.CanUseWithoutFlavors = value;
  }

  public string DefaultFlavor
  {
    get => _provider.DefaultFlavor;
    set => _provider.DefaultFlavor = value;
  }

  public Type BaseType => _provider.BaseType;

  public Func<object> FlavorlessConstruct
  {
    get => _provider.FlavorlessConstruct;
    set => _provider.FlavorlessConstruct = value;
  }

  public Type FlavorlessType
  {
    get => _provider.FlavorlessType;
    set => _provider.FlavorlessType = value;
  }

  public IEnumerable<FlavorConfig> GetAll() => _options.GetAll<TEntity>();

  public FlavorConfig Get<TFlavor>() => _options.Get(typeof(TEntity), typeof(TFlavor));

  public FlavorConfig Get<TFlavor>(string alias) => _options.Get(typeof(TEntity), alias);

  public FlavorConfig Get(Type flavorType) => _options.Get(typeof(TEntity), flavorType);

  public FlavorConfig Get(string alias) => _options.Get(typeof(TEntity), alias);

  public bool Exists(string alias) => _options.Exists<TEntity>(alias);

  public void Add<TFlavor>(string alias, string name, string description = null, string icon = null) 
    where TFlavor : TEntity, new() => _options.Add<TEntity, TFlavor>(alias, name, description, icon);

  public void Add<TFlavor>(Func<FlavorConfig, TFlavor> construct, string alias, string name, string description = null, string icon = null)
    where TFlavor : TEntity, new() => _options.Add<TEntity, TFlavor>(construct, alias, name, description, icon);

  public void Add<TFlavor>(Type flavorType, Func<FlavorConfig, TFlavor> construct, string alias, string name, string description = null, string icon = null)
    where TFlavor : TEntity, new() => _options.Add<TEntity, TFlavor>(flavorType, construct, alias, name, description, icon);

  public void Add<TFlavor>(FlavorConfig config)
    where TFlavor : TEntity, new() => _options.Add<TEntity, TFlavor>(config);

  public void Remove(string alias)
  {
    FlavorProvider provider = _options.Providers.GetValueOrDefault(_baseType, new());
    FlavorConfig flavor = provider.Flavors.FirstOrDefault(x => x.Alias == alias);

    if (flavor != null)
    {
      provider.Flavors.Remove(flavor);
    }
  }

  public void RemoveAll()
  {
    FlavorProvider provider = _options.Providers.GetValueOrDefault(_baseType, new());
    provider.Flavors.Clear();
  }
}
