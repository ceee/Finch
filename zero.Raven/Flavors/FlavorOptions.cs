using System.Collections.Concurrent;

namespace zero.Raven;

public class FlavorOptions
{
  public ConcurrentDictionary<Type, FlavorProvider> Providers { get; private set; } = new();

  public void Configure<TEntity>(Action<FlavorProviderOptions<TEntity>> configure) where TEntity : class, ISupportsFlavors, new()
  {
    Type type = typeof(TEntity);
    FlavorProvider provider = Providers.GetOrAdd(type, _ => CreateProvider<TEntity>());
    configure(new FlavorProviderOptions<TEntity>(this, provider));
  }


  public bool CanUseWithoutFlavors<TEntity>() where TEntity : class, ISupportsFlavors, new()
  {
    return Providers.GetValueOrDefault(typeof(TEntity), new()).CanUseWithoutFlavors;
  }

  public string DefaultFlavorFor<TEntity>() where TEntity : class, ISupportsFlavors, new()
  {
    FlavorProvider provider = Providers.GetValueOrDefault(typeof(TEntity), new());

    if (!provider.Flavors.Any())
    {
      return null;
    }

    if (provider.DefaultFlavor.HasValue() && provider.Flavors.Any(x => x.Alias == provider.DefaultFlavor))
    {
      return provider.DefaultFlavor;
    }

    return null;
  }


  public TFlavor Construct<TEntity, TFlavor>(string alias = null) 
    where TEntity : class, ISupportsFlavors, new()
    where TFlavor : class, TEntity, new()
  {
    // if no flavor provider is registered we return the base entity
    if (!Providers.TryGetValue(typeof(TEntity), out FlavorProvider provider))
    {
      return new TFlavor();
    }

    // throw if this entity is not allowed to be created without a flavor
    if (alias.IsNullOrEmpty() && !provider.CanUseWithoutFlavors)
    {
      string defaultFlavorAlias = provider.DefaultFlavor;

      if (defaultFlavorAlias.IsNullOrEmpty() || !provider.Flavors.Any(x => x.Alias == defaultFlavorAlias))
      {
        throw new ArgumentException("Can not create instance of an entity which is configured to to be only used as a flavor", nameof(alias));
      }

      alias = defaultFlavorAlias;
    }

    // return default instance if no flavor is required
    if (alias.IsNullOrEmpty())
    {
      return provider.FlavorlessConstruct() as TFlavor;
    }

    // try to load and construct a specific flavor
    FlavorConfig config = provider.Flavors.FirstOrDefault(x => x.Alias == alias);
    TFlavor result = config?.Construct(config) as TFlavor;

    if (result == null)
    {
      return default;
    }

    result.Flavor = alias;
    return result;
  }


  public IEnumerable<FlavorConfig> GetAll<TEntity>() => GetAll(typeof(TEntity));


  public IEnumerable<FlavorConfig> GetAll(Type type) => Providers.GetValueOrDefault(type, new()).Flavors;


  public FlavorConfig Get<TEntity, TFlavor>() => Get(typeof(TEntity), typeof(TFlavor));


  public FlavorConfig Get<TEntity, TFlavor>(string alias) => Get(typeof(TEntity), typeof(TFlavor), alias);


  public FlavorConfig Get<TEntity>(string alias) => Get(typeof(TEntity), alias);


  public FlavorConfig Get(Type baseType, Type flavorType)
  {
    FlavorProvider provider = Providers.GetValueOrDefault(baseType, new());
    return provider.Flavors.FirstOrDefault(x => x.FlavorType == flavorType);
  }


  public FlavorConfig Get(Type baseType, Type flavorType, string alias)
  {
    FlavorProvider provider = Providers.GetValueOrDefault(baseType, new());
    return provider.Flavors.FirstOrDefault(x => x.FlavorType == flavorType && x.Alias == alias);
  }


  public FlavorConfig Get(Type baseType, string alias)
  {
    FlavorProvider provider = Providers.GetValueOrDefault(baseType, new());
    return provider.Flavors.FirstOrDefault(x => x.Alias == alias);
  }


  public bool Exists<TEntity>(string alias)
  {
    FlavorProvider provider = Providers.GetValueOrDefault(typeof(TEntity), new());
    return provider.Flavors.Any(x => x.Alias == alias);
  }


  public void Add<TEntity, TFlavor>(string alias, string name, string description = null, string icon = null) 
    where TEntity : class, ISupportsFlavors, new()
    where TFlavor : TEntity, new()
  {
    Add<TEntity, TFlavor>(typeof(TFlavor), _ => new TFlavor(), alias, name, description, icon);
  }


  public void Add<TEntity, TFlavor>(Func<FlavorConfig, TFlavor> construct, string alias, string name, string description = null, string icon = null)
    where TEntity : class, ISupportsFlavors, new()
    where TFlavor : TEntity, new()
  {
    Add<TEntity, TFlavor>(typeof(TFlavor), construct, alias, name, description, icon);
  }


  public void Add<TEntity, TFlavor>(Type flavorType, Func<FlavorConfig, TFlavor> construct, string alias, string name, string description = null, string icon = null)
    where TEntity : class, ISupportsFlavors, new()
    where TFlavor : TEntity, new()
  {
    Add<TEntity, TFlavor>(new(flavorType)
    {
      Alias = alias,
      Name = name,
      Description = description,
      Icon = icon,
      Construct = _ => construct(_)
    });
  }


  public void Add<TEntity, TFlavor>(FlavorConfig config)
    where TEntity : class, ISupportsFlavors, new()
    where TFlavor : TEntity, new()
  {
    Type baseEntityType = typeof(TEntity);
    FlavorProvider provider = Providers.GetOrAdd(baseEntityType, _ => CreateProvider<TEntity>());
    provider.Flavors.Add(config);
  }


  public void Implement<TEntity, TDefaultImplementation>()
    where TEntity : class, ISupportsFlavors, new()
    where TDefaultImplementation : TEntity, new()
  {
    Type baseEntityType = typeof(TEntity);
    FlavorProvider provider = Providers.GetOrAdd(baseEntityType, _ => CreateProvider<TEntity>());

    provider.FlavorlessConstruct = () => new TDefaultImplementation();
    provider.FlavorlessType = typeof(TDefaultImplementation);
  }


  FlavorProvider CreateProvider<TEntity>()
    where TEntity : class, ISupportsFlavors, new()
  {
    return new FlavorProvider()
    {
      BaseType = typeof(TEntity),
      FlavorlessConstruct = () => new TEntity(),
      FlavorlessType = typeof(TEntity),
      ConverterCreator = cfg => new JsonFlavorVariantConverter<TEntity>(cfg)
    };
  }
}
