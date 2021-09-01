using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using zero.Core.Database;

namespace zero.Core.Options
{
  public class RavenOptions
  {
    public string Url { get; set; }

    public string Database { get; set; }

    public string CollectionPrefix { get; set; }

    public RavenIndexesOptions Indexes { get; set; } = new();
  }


  public class RavenIndexesOptions : ZeroBackofficeCollection<Type>, IZeroCollectionOptions
  {
    public RavenIndexModifiersOptions Modifiers { get; private set; } = new();

    public void Add<T>() where T : IZeroIndexDefinition, new()
    {
      Items.Add(typeof(T));
    }

    public void Add(Type indexType)
    {
      Items.Add(indexType);
    }

    public void AddRange(params Type[] indexes)
    {
      Items.AddRange(indexes);
    }

    public void Replace<T, TReplaceWith>()
       where T : IZeroIndexDefinition, new()
       where TReplaceWith : IZeroIndexDefinition, new()
    {
      Items.Remove(typeof(T));
      Items.Add(typeof(TReplaceWith));
    }

    public void Replace(Type origin, Type replaceWith)
    {
      Items.Remove(origin);
      Items.Add(replaceWith);
    }

    public IEnumerable<IZeroIndexDefinition> GetAllForRegistration(IZeroOptions options)
    {
      foreach (Type type in Items)
      {
        IZeroIndexDefinition index = (IZeroIndexDefinition)Activator.CreateInstance(type);
        index.Setup(options);
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
