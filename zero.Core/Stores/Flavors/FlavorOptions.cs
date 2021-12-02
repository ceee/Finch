using System.Collections.Concurrent;

namespace zero.Stores;

public class FlavorOptions
{
  readonly ConcurrentDictionary<Type, List<FlavorConfig>> Flavors = new();


  public IEnumerable<FlavorConfig> GetAll<TEntity>()
  {
    return GetAll(typeof(TEntity));
  }


  public IEnumerable<FlavorConfig> GetAll(Type type)
  {
    return Flavors.GetValueOrDefault(type, new());
  }


  public FlavorConfig Get<TEntity, TFlavor>()
  {
    return Get(typeof(TEntity), typeof(TFlavor));
  }


  public FlavorConfig Get<TEntity>(string alias)
  {
    return Get(typeof(TEntity), alias);
  }


  public FlavorConfig Get(Type baseType, Type flavorType)
  {
    List<FlavorConfig> flavors = Flavors.GetValueOrDefault(baseType, new());
    return flavors.FirstOrDefault(x => x.FlavorType == flavorType);
  }


  public FlavorConfig Get(Type baseType, string alias)
  {
    List<FlavorConfig> flavors = Flavors.GetValueOrDefault(baseType, new());
    return flavors.FirstOrDefault(x => x.Alias == alias);
  }


  public bool Exists<TEntity>(string alias)
  {
    List<FlavorConfig> flavors = Flavors.GetValueOrDefault(typeof(TEntity), new());
    return flavors.Any(x => x.Alias == alias);
  }


  public void Provide<TEntity>()
    where TEntity : ISupportsFlavors, new()
  {
    Provide(_ => new TEntity());
  }


  public void Provide<TEntity>(Func<FlavorConfig, TEntity> construct)
    where TEntity : ISupportsFlavors
  {
    Type type = typeof(TEntity);

    if (Flavors.ContainsKey(type))
    {
      return;
    }

    FlavorProviderConfig config = new(type)
    {
      Construct = _ => construct(_)
    };

    if (!Flavors.TryAdd(type, new() { config }))
    {
      throw new Exception($"Could not add flavor provider '{type.Name}'");
    }
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

    if (!Flavors.ContainsKey(baseEntityType))
    {
      Provide<TEntity>();
    }

    Flavors[baseEntityType].Add(config);
  }
}
