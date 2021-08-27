using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public IEnumerable<IAbstractIndexCreationTask> GetAllForRegistration()
    {
      foreach (Type type in Items)
      {
        yield return (IAbstractIndexCreationTask)Activator.CreateInstance(type);
      }
    }
  }
}
