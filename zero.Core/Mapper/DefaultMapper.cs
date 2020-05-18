using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Options;

namespace zero.Core.Mapper
{
  public class DefaultMapper : IMapper
  {
    MapCache Maps = new MapCache();

    IDocumentStore Raven = null;

    IZeroOptions Options = null;


    public DefaultMapper(IDocumentStore raven, IZeroOptions options)
    {
      Raven = raven;
      Options = options;

      foreach (IMapperConfig config in options.Mapper.GetAllItems())
      {
        config.Configure(this);
      }
    }


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
    public async Task<TTarget> Map<TSource, TTarget>(TSource source) where TTarget : class, new()
    {
      if (source == null)
      {
        return null;
      }

      return await Map(source, new TTarget());
    }


    /// <inheritdoc />
    public async Task<TTarget> Map<TSource, TTarget>(TSource source, TTarget target) where TTarget : class, new()
    {
      if (source == null)
      {
        return target;
      }

      if (target == null)
      {
        return await Map<TSource, TTarget>(source);
      }

      await Maps.Call(source, target);

      return target;
    }


    /// <inheritdoc />
    public async Task<IEnumerable<TTarget>> Map<TSource, TTarget>(IEnumerable<TSource> source) where TTarget : class, new()
    {
      IList<TTarget> target = new List<TTarget>();

      foreach (TSource item in source)
      {
        target.Add(await Map(item, new TTarget()));
      }

      return target;
    }


    /// <inheritdoc />
    public async Task<ListResult<TTarget>> Map<TSource, TTarget>(ListResult<TSource> source) where TTarget : class, new()
    {
      IList<TTarget> target = new List<TTarget>();

      foreach (TSource item in source.Items)
      {
        target.Add(await Map(item, new TTarget()));
      }

      return new ListResult<TTarget>(target, source.TotalItems, source.Page, source.PageSize)
      {
        Statistics = source.Statistics
      };
    }


    /// <inheritdoc />
    public async Task<EntityResult<TTarget>> Map<TSource, TTarget>(EntityResult<TSource> source) where TTarget : class, new()
    {
      return new EntityResult<TTarget>()
      {
        IsSuccess = source.IsSuccess,
        Errors = source.Errors,
        Model = await Map(source.Model, new TTarget())
      };
    }


    /// <inheritdoc />
    public void CreateMap<TSource, TTarget>(Action<TSource, TTarget> map) where TTarget : class, new()
    {
      Maps.Add<TSource, TTarget>((source, target) =>
      {
        map((TSource)source, (TTarget)target);
        return Task.CompletedTask;
      });
    }


    /// <inheritdoc />
    public void CreateMap<TSource, TTarget>(Func<TSource, TTarget, IAsyncDocumentSession, Task> map) where TTarget : class, new()
    {   
      Maps.Add<TSource, TTarget>(async (source, target) =>
      {
        await map((TSource)source, (TTarget)target, null);
      });
    }


    /// <summary>
    /// Internal mappings cache
    /// </summary>
    class MapCache : Dictionary<int, Dictionary<int, Func<object, object, Task>>>
    {
      int Index = 0;

      Dictionary<string, int> TypeMappings = new Dictionary<string, int>();


      /// <summary>
      /// Adds a new action for the mapping
      /// </summary>
      public void Add<TSource, TTarget>(Func<object, object, Task> map)
      {
        int sourceKey = GetKeyForType(typeof(TSource));
        int targetKey = GetKeyForType(typeof(TTarget));

        if (!ContainsKey(sourceKey))
        {
          Add(sourceKey, new Dictionary<int, Func<object, object, Task>>());
        }

        if (!this[sourceKey].ContainsKey(targetKey))
        {
          this[sourceKey].Add(targetKey, map);
        }
      }


      /// <summary>
      /// Get action from defined types
      /// </summary>
      public async Task Call<TSource, TTarget>(TSource source, TTarget target)
      {
        int sourceKey = GetKeyForType(typeof(TSource));
        int targetKey = GetKeyForType(typeof(TTarget));

        if (!ContainsKey(sourceKey) || !this[sourceKey].ContainsKey(targetKey))
        {
          return;
        }

        Func<object, object, Task> result = this[sourceKey][targetKey];

        await result(source, target);
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
    Task<TTarget> Map<TSource, TTarget>(TSource source) where TTarget : class, new();

    /// <summary>
    /// Map an object to the target type given an already existing target instance
    /// </summary>
    Task<TTarget> Map<TSource, TTarget>(TSource source, TTarget target) where TTarget : class, new();

    /// <summary>
    /// Map a list of objects to the target type
    /// </summary>
    Task<IEnumerable<TTarget>> Map<TSource, TTarget>(IEnumerable<TSource> source) where TTarget : class, new();

    /// <summary>
    /// Map a list result containing objects to the target type
    /// </summary>
    Task<ListResult<TTarget>> Map<TSource, TTarget>(ListResult<TSource> source) where TTarget : class, new();

    /// <summary>
    /// Map an entity result to the target type
    /// </summary>
    Task<EntityResult<TTarget>> Map<TSource, TTarget>(EntityResult<TSource> source) where TTarget : class, new();

    /// <summary>
    /// Create a mapping from source to target object
    /// </summary>
    void CreateMap<TSource, TTarget>(Action<TSource, TTarget> map) where TTarget : class, new();

    /// <inheritdoc />
    void CreateMap<TSource, TTarget>(Func<TSource, TTarget, IAsyncDocumentSession, Task> map) where TTarget : class, new();
  }
}