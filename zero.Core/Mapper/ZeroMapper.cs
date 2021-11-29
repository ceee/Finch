using System.Collections.Concurrent;

namespace zero.Mapper;

public class ZeroMapper : IZeroMapper
{
  /// <summary>
  /// Concurrent cache for all ctor definitions
  /// </summary>
  protected ConcurrentDictionary<Type, Dictionary<Type, Action<object, object, IZeroMapperContext>>> MapDefinitions { get; private set; } = new();

  /// <summary>
  /// Concurrent cache for all constructor definitions
  /// </summary>
  protected ConcurrentDictionary<Type, Dictionary<Type, Func<object, IZeroMapperContext, object>>> ConstructorDefinitions { get; private set; } = new();


  public ZeroMapper(IEnumerable<IMapperProfile> profiles)
  {
    foreach (IMapperProfile profile in profiles)
    {
      profile.Configure(this);
    }
  }


  /// <inheritdoc />
  public void Define<TSource, TDestination>(Func<TSource, IZeroMapperContext, TDestination> ctor, Action<TSource, TDestination, IZeroMapperContext> map)
  {
    Type sourceType = typeof(TSource);
    Type destinationType = typeof(TDestination);

    var sourceMaps = MapDefinitions.GetOrAdd(sourceType, _ => new());
    var sourceCtors = ConstructorDefinitions.GetOrAdd(sourceType, _ => new());

    sourceCtors[destinationType] = (source, ctx) => ctor((TSource)source, ctx);
    sourceMaps[destinationType] = (source, destination, ctx) => map((TSource)source, (TDestination)destination, ctx);
  }


  /// <inheritdoc />
  public TDestination Map<TDestination>(object source, Type sourceType, TDestination destination = default)
  {
    if (source == null)
    {
      return default;
    }

    Type destinationType = typeof(TDestination);
    ZeroMapperContext mapperContext = new(this);

    var constructor = GetConstructor(sourceType, destinationType);
    var map = GetMap(sourceType, destinationType);

    if (constructor != null && map != null)
    {
      destination ??= (TDestination)constructor(source, mapperContext);

      map(source, destination, mapperContext);

      return destination;
    }

    // TODO enumerables

    throw new InvalidOperationException($"Don't know how to map {sourceType.FullName} to {destinationType.FullName}.");
  }


  protected virtual Func<object, IZeroMapperContext, object> GetConstructor(Type sourceType, Type targetType)
  {
    if (ConstructorDefinitions.TryGetValue(sourceType, out var sourceCtor) && sourceCtor.TryGetValue(targetType, out var ctor))
    {
      return ctor;
    }

    // we *may* run this more than once but it does not matter

    ctor = null;
    foreach (var (stype, sctors) in ConstructorDefinitions)
    {
      if (!stype.IsAssignableFrom(sourceType)) continue;
      if (!sctors.TryGetValue(targetType, out ctor)) continue;

      sourceCtor = sctors;
      break;
    }

    if (ctor == null)
    {
      return null;
    }

    ConstructorDefinitions.AddOrUpdate(sourceType, sourceCtor, (k, v) =>
    {
      // Add missing constructors
      foreach (var c in sourceCtor)
      {
        if (!v.ContainsKey(c.Key))
        {
          v.Add(c.Key, c.Value);
        }
      }

      return v;
    });


    return ctor;
  }

  protected virtual Action<object, object, IZeroMapperContext> GetMap(Type sourceType, Type targetType)
  {
    if (MapDefinitions.TryGetValue(sourceType, out var sourceMap) && sourceMap.TryGetValue(targetType, out var map))
    {
      return map;
    }

    // we *may* run this more than once but it does not matter

    map = null;
    foreach (var (stype, smap) in MapDefinitions)
    {
      if (!stype.IsAssignableFrom(sourceType)) continue;

      // TODO: consider looking for assignable types for target too?
      if (!smap.TryGetValue(targetType, out map)) continue;

      sourceMap = smap;
      break;
    }

    if (map == null) return null;

    if (MapDefinitions.ContainsKey(sourceType))
    {
      foreach (var m in sourceMap)
      {
        if (!MapDefinitions[sourceType].TryGetValue(m.Key, out _))
        {
          MapDefinitions[sourceType].Add(m.Key, m.Value);
        }
      }
    }
    else
    {
      MapDefinitions[sourceType] = sourceMap;
    }

    return map;
  }
}


public interface IZeroMapper
{
  /// <summary>
  /// Register a new mapping
  /// </summary>
  void Define<TSource, TDestination>(Func<TSource, IZeroMapperContext, TDestination> ctor, Action<TSource, TDestination, IZeroMapperContext> map);

  /// <summary>
  /// Map a source type to the destination type
  /// </summary>
  TDestination Map<TDestination>(object source, Type sourceType, TDestination destination = default);
}