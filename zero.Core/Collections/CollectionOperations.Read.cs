using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace zero.Collections;

public abstract partial class CollectionOperations
{
  /// <inheritdoc />
  public virtual async Task<T> Load<T>(string id, string changeVector = null) where T : ZeroIdEntity, new()
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
  public virtual async Task<Dictionary<string, T>> Load<T>(IEnumerable<string> ids) where T : ZeroIdEntity, new()
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
  public virtual async Task<Paged<T>> Load<T>(int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default) where T : ZeroIdEntity, new()
  {
    IRavenQueryable<T> queryable = Session.Query<T>().Statistics(out QueryStatistics statistics);
    querySelector ??= x => x;

    List<T> result = await querySelector(queryable).Paging(pageNumber, pageSize).ToListAsync();
    return new Paged<T>(result, statistics.TotalResults, pageNumber, pageSize);
  }


  /// <inheritdoc />
  public virtual async Task<Paged<T>> Load<T, TIndex>(int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default) 
    where T : ZeroIdEntity, new() 
    where TIndex : AbstractCommonApiForIndexes, new()
  {
    IRavenQueryable<T> queryable = Session.Query<T, TIndex>().Statistics(out QueryStatistics statistics);
    querySelector ??= x => x;

    List<T> result = await querySelector(queryable).Paging(pageNumber, pageSize).ToListAsync();
    return new Paged<T>(result, statistics.TotalResults, pageNumber, pageSize);
  }


  /// <inheritdoc />
  public virtual async Task<List<T>> LoadAll<T>() where T : ZeroIdEntity, new()
  {
    List<T> items = new();

    await foreach (T item in Stream<T>())
    {
      items.Add(item);
    }

    return items;
  }


  /// <inheritdoc />
  public virtual IAsyncEnumerable<T> Stream<T>() where T : ZeroIdEntity, new() => Stream<T>(null);


  /// <inheritdoc />
  public virtual async IAsyncEnumerable<T> Stream<T>(Func<IRavenQueryable<T>, IQueryable<T>> expression) where T : ZeroIdEntity, new()
  {
    IRavenQueryable<T> query = Session.Query<T>();
    IQueryable<T> queryable = query;

    if (expression != null)
    {
      queryable = expression(query);
    }

    var stream = await Session.Advanced.StreamAsync(queryable);

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