using System.Collections.Concurrent;

namespace zero.Stores;

public class FlavorOptions
{
  public ConcurrentDictionary<Type, FlavorProvider> Providers { get; private set; } = new();


  public void Configure<TEntity>(Action<FlavorProviderOptions<TEntity>> configure) where TEntity : ISupportsFlavors, new()
  {
    Type type = typeof(TEntity);
    FlavorProvider provider = Providers.GetOrAdd(type, _ => new() { BaseType = type });
    configure(new FlavorProviderOptions<TEntity>(this, provider));
  }


  public bool CanUseWithoutFlavors<TEntity>() where TEntity : ISupportsFlavors, new()
  {
    return Providers.GetValueOrDefault(typeof(TEntity), new()).CanUseWithoutFlavors;
  }

  public string DefaultFlavorFor<TEntity>() where TEntity : ISupportsFlavors, new()
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

    return provider.Flavors.First().Alias;
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
    where TEntity : ISupportsFlavors, new()
    where TFlavor : TEntity, new()
  {
    Add<TEntity, TFlavor>(typeof(TFlavor), _ => new TFlavor(), alias, name, description, icon);
  }


  public void Add<TEntity, TFlavor>(Func<FlavorConfig, TFlavor> construct, string alias, string name, string description = null, string icon = null)
    where TEntity : ISupportsFlavors, new()
    where TFlavor : TEntity, new()
  {
    Add<TEntity, TFlavor>(typeof(TFlavor), construct, alias, name, description, icon);
  }


  public void Add<TEntity, TFlavor>(Type flavorType, Func<FlavorConfig, TFlavor> construct, string alias, string name, string description = null, string icon = null)
    where TEntity : ISupportsFlavors, new()
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
    where TEntity : ISupportsFlavors, new()
    where TFlavor : TEntity, new()
  {
    Type baseEntityType = typeof(TEntity);
    FlavorProvider provider = Providers.GetOrAdd(baseEntityType, _ => new() { BaseType = baseEntityType });
    provider.Flavors.Add(config);
  }
}
