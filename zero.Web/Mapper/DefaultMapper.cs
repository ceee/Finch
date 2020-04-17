using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace zero.Web.Mapper
{
  public class DefaultMapper : IMapper
  {
    MapCache Maps = new MapCache();


    /// <inheritdoc />
    public void Add<T>() where T : IMapperConfig, new()
    {
      T config = new T();
      config.Configure(this);
    }


    /// <inheritdoc />
    public void Add(Assembly assembly)
    {
      Type configType = typeof(IMapperConfig);
      IEnumerable<Type> types = assembly.GetTypes().Where(t => t.IsAssignableFrom(configType));

      foreach (Type type in types)
      {
        IMapperConfig config = (IMapperConfig)Activator.CreateInstance(type);
        config.Configure(this);
      }
    }


    /// <inheritdoc />
    public TTarget Map<TSource, TTarget>(TSource source) where TTarget : class, new()
    {
      if (source == null)
      {
        return null;
      }

      return Map(source, new TTarget());
    }


    /// <inheritdoc />
    public TTarget Map<TSource, TTarget>(TSource source, TTarget target) where TTarget : class, new()
    {
      if (source == null)
      {
        return target;
      }

      Maps.Call(source, target);

      return target;
    }


    /// <inheritdoc />
    public void CreateMap<TSource, TTarget>(Action<TSource, TTarget> map) where TTarget : class, new()
    {
      Maps.Add<TSource, TTarget>((source, target) =>  map((TSource)source, (TTarget)target));
    }


    /// <summary>
    /// Internal mappings cache
    /// </summary>
    class MapCache : Dictionary<int, Dictionary<int, Action<object, object>>>
    {
      int Index = 0;

      Dictionary<string, int> TypeMappings = new Dictionary<string, int>();


      /// <summary>
      /// Adds a new action for the mapping
      /// </summary>
      public void Add<TSource, TTarget>(Action<object, object> map)
      {
        int sourceKey = GetKeyForType(typeof(TSource));
        int targetKey = GetKeyForType(typeof(TTarget));

        if (!ContainsKey(sourceKey))
        {
          Add(sourceKey, new Dictionary<int, Action<object, object>>());
        }

        if (!this[sourceKey].ContainsKey(targetKey))
        {
          this[sourceKey].Add(targetKey, map);
        }
      }


      /// <summary>
      /// Get action from defined types
      /// </summary>
      public void Call<TSource, TTarget>(TSource source, TTarget target)
      {
        int sourceKey = GetKeyForType(typeof(TSource));
        int targetKey = GetKeyForType(typeof(TTarget));

        if (!ContainsKey(sourceKey) || !this[sourceKey].ContainsKey(targetKey))
        {
          return;
        }

        Action<object, object> result = this[sourceKey][targetKey];

        result(source, target);
      }


      /// <summary>
      /// Get stored key for this type
      /// </summary>
      int GetKeyForType(Type type)
      {
        string name = type.FullName;

        if (TypeMappings.TryGetValue(name, out int key))
        {
          return key;
        }

        int index = Index++;
        TypeMappings.Add(name, index);
        return index;
      }
    }
  }


  public interface IMapper
  {
    /// <inheritdoc />
    void Add<T>() where T : IMapperConfig, new();

    /// <inheritdoc />
    void Add(Assembly assembly);

    /// <summary>
    /// Map an object to the target type
    /// </summary>
    TTarget Map<TSource, TTarget>(TSource source) where TTarget : class, new();

    /// <summary>
    /// Map an object to the target type given an already existing target instance
    /// </summary>
    TTarget Map<TSource, TTarget>(TSource source, TTarget target) where TTarget : class, new();

    /// <summary>
    /// Create a mapping from source to target object
    /// </summary>
    void CreateMap<TSource, TTarget>(Action<TSource, TTarget> map) where TTarget : class, new();
  }
}