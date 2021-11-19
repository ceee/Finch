using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using zero.Core.Database;
using zero.Core.Extensions;

namespace zero.Core.Options
{
  public class RavenOptions
  {
    public string Url { get; set; }

    public string Database { get; set; }

    public string CollectionPrefix { get; set; }

    public RavenIndexesOptions Indexes { get; set; } = new();
  }


  public class RavenIndexesOptions : ZeroBackofficeCollection<RavenIndexesOptions.Map>, IZeroCollectionOptions
  {
    public class Map
    {
      internal Type Type { get; set; }

      internal Expression<Func<IZeroIndexDefinition>> CreateIndex { get; set; }

      internal Map(Type type, Expression<Func<IZeroIndexDefinition>> create)
      {
        Type = type;
        CreateIndex = create;
      }
    }


    public RavenIndexModifiersOptions Modifiers { get; private set; } = new();

    public void Add<T>() where T : IZeroIndexDefinition, new()
    {
      Items.Add(new(typeof(T), () => new T()));
    }

    public void Add(Type indexType)
    {
      Items.Add(new(indexType, () => (IZeroIndexDefinition)Activator.CreateInstance(indexType)));
    }

    public void Add<T>(T index) where T : IZeroIndexDefinition
    {
      Items.Add(new(typeof(T), () => index));
    }

    public void AddRange(params Type[] indexes)
    {
      foreach (Type type in indexes)
      {
        Add(type);
      }
    }

    public void Replace<T, TReplaceWith>()
       where T : IZeroIndexDefinition, new()
       where TReplaceWith : IZeroIndexDefinition, new()
    {
      Replace(typeof(T), typeof(TReplaceWith));
    }

    public void Replace(Type origin, Type replaceWith)
    {
      var item = Items.FirstOrDefault(x => x.Type == origin);
      if (item != null)
      {
        Items.Remove(item);
      }
      Add(replaceWith);
    }

    public IEnumerable<IZeroIndexDefinition> BuildAll(IZeroOptions options, IDocumentStore store)
    {
      foreach (Map map in Items)
      {
        IZeroIndexDefinition index = map.CreateIndex.Compile().Invoke();
        index.Setup(options, store);
        index.RunModifiers(options);
        yield return index;
      }
    }
  }


  public class RavenIndexModifiersOptions : ZeroBackofficeCollection<RavenIndexModifiersOptions.Modifier>, IZeroCollectionOptions
  {
    public class Modifier
    {
      public Type Type { get; set; }

      public Expression<Action<IZeroIndexDefinition>> Modify { get; set; }
    }

    public void Add<T>(Action<T> modify) where T : IZeroIndexDefinition, new()
    {
      Items.Add(new()
      {
        Type = typeof(T),
        Modify = x => modify((T)x)
      });
    }


    public IEnumerable<Modifier> GetAllForType<T>() where T : IZeroIndexDefinition, new() => GetAllForType(typeof(T));


    public IEnumerable<Modifier> GetAllForType(Type type)
    {
      foreach (Modifier modifier in Items.Where(x => x.Type.IsAssignableFrom(type)))
      {
        yield return modifier;
      }
    }
  }
}
