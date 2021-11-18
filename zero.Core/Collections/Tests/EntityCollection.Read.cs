namespace zero.Core;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

public abstract partial class EntityCollection<T> : IEntityCollection<T> where T : ZeroIdEntity
{
  /// <inheritdoc />
  public virtual async Task<T> Load(string id, string changeVector = null)
  {
    if (id.IsNullOrWhiteSpace())
    {
      return default;
    }
    if (!changeVector.IsNullOrEmpty())
    {
      //return WhenActive(await GetRevision(changeVector)); // TODO
    }

    return WhenActive(await Session.LoadAsync<T>(id));
  }


  /// <inheritdoc />
  public virtual async Task<Dictionary<string, T>> Load(params string[] ids)
  {
    ids = ids.Distinct().ToArray();

    Dictionary<string, T> models = await Session.LoadAsync<T>(ids);
    Dictionary<string, T> result = new();

    foreach (string id in ids)
    {
      models.TryGetValue(id, out T model);
      result.Add(id, WhenActive(model));
    }

    return result;
  }


  /// <inheritdoc />
  public virtual async Task<ListResult<T>> Load(ListQuery<T> query)
  {
    return await Session.Query<T>().ToQueriedListAsyncX(query); // TODO whenActive + AsyncX update for ZeroEntity
  }


  /// <inheritdoc />
  public virtual async Task<ListResult<T>> Load<TIndex>(ListQuery<T> query) where TIndex : AbstractCommonApiForIndexes, new()
  {
    return await Session.Query<T, TIndex>().ToQueriedListAsyncX(query);
  }


  /// <inheritdoc />
  public virtual async Task<List<T>> LoadAll()
  {
    List<T> items = new();

    await foreach (T item in Stream())
    {
      items.Add(item);
    }

    return items;
  }


  /// <inheritdoc />
  public virtual IAsyncEnumerable<T> Stream() => Stream(null);


  /// <inheritdoc />
  public virtual async IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IRavenQueryable<T>> expression)
  {
    IRavenQueryable<T> query = Session.Query<T>();

    if (expression != null)
    {
      query = expression(query);
    }

    var stream = await Session.Advanced.StreamAsync(query);

    while (await stream.MoveNextAsync())
    {
      if (WhenActive(stream.Current.Document) == default)
      {
        continue;
      }

      yield return stream.Current.Document;
    }
  }
}