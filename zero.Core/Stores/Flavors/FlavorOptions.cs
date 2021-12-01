using System.Collections.Concurrent;

namespace zero.Pages;

public class FlavorOptions
{
  readonly ConcurrentDictionary<Type, List<FlavorConfig>> Flavors = new();


  public void Provide<TEntity>(string alias, string name, string description = null, string icon = null)
    where TEntity : ISupportsFlavors, new()
  {
    Provide(_ => new TEntity(), alias, name, description, icon);
  }


  public void Provide<TEntity>(Func<FlavorConfig, TEntity> construct, string alias, string name, string description = null, string icon = null)
    where TEntity : ISupportsFlavors
  {
    Type type = typeof(TEntity);

    if (Flavors.ContainsKey(type))
    {
      throw new KeyNotFoundException($"Already a provider for type '{type.Name}' registered.");
    }

    FlavorProviderConfig config = new(type)
    {
      Alias = alias,
      Name = name,
      Description = description,
      Icon = icon,
      Construct = _ => construct(_)
    };

    if (!Flavors.TryAdd(type, new() { config }))
    {
      throw new Exception($"Could not add flavor provider '{type.Name}'");
    }
  }


  public void Add<TEntity, TFlavor>(string alias, string name, string description = null, string icon = null) 
    where TEntity : ISupportsFlavors
    where TFlavor : TEntity, new()
  {
    Add(typeof(TEntity), typeof(TFlavor), _ => new TFlavor(), alias, name, description, icon);
  }


  public void Add<TEntity, TFlavor>(Func<FlavorConfig, TFlavor> construct, string alias, string name, string description = null, string icon = null)
    where TEntity : ISupportsFlavors
    where TFlavor : TEntity, new()
  {
    Add(typeof(TEntity), typeof(TFlavor), construct, alias, name, description, icon);
  }


  void Add<TFlavor>(Type baseEntityType, Type flavorType, Func<FlavorConfig, TFlavor> construct, string alias, string name, string description = null, string icon = null)
  {
    if (!Flavors.ContainsKey(baseEntityType))
    {
      throw new KeyNotFoundException($"No provider for type '{baseEntityType.Name}' found. Use Provide<>() first.");
    }

    FlavorConfig config = new(flavorType)
    {
      Alias = alias,
      Name = name,
      Description = description,
      Icon = icon,
      Construct = _ => construct(_)
    };

    Flavors[baseEntityType].Add(config);
  }
}
