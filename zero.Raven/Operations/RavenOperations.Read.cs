using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Linq.Expressions;

namespace zero.Raven;

public partial class RavenOperations : IRavenOperations
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
  public virtual async Task<bool> Any<T>(Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default) where T : ZeroIdEntity, new()
  {
    querySelector ??= x => x;
    return await querySelector(Session.Query<T>()).AnyAsync();
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
  public virtual async Task<List<T>> Load<T>(Func<IRavenQueryable<T>, IQueryable<T>> querySelector) where T : ZeroIdEntity, new()
  {
    IRavenQueryable<T> queryable = Session.Query<T>();
    querySelector ??= x => x;

    return await querySelector(queryable).ToListAsync();
  }


  /// <inheritdoc />
  public virtual async Task<List<T>> Load<T, TIndex>(Func<IRavenQueryable<T>, IQueryable<T>> querySelector)
    where T : ZeroIdEntity, new()
    where TIndex : AbstractCommonApiForIndexes, new()
  {
    IRavenQueryable<T> queryable = Session.Query<T, TIndex>();
    querySelector ??= x => x;

    return await querySelector(queryable).ToListAsync();
  }


  /// <inheritdoc />
  public virtual async Task<List<T>> Load<T>(Expression<Func<T, bool>> predicate) where T : ZeroIdEntity, new()
  {
    return await Session.Query<T>().Where(predicate).ToListAsync();
  }


  /// <inheritdoc />
  public virtual async Task<List<T>> Load<T, TIndex>(Expression<Func<T, bool>> predicate)
    where T : ZeroIdEntity, new()
    where TIndex : AbstractCommonApiForIndexes, new()
  {
    return await Session.Query<T, TIndex>().Where(predicate).ToListAsync();
  }


  /// <inheritdoc />
  public virtual async Task<List<T>> LoadAll<T>() where T : ZeroIdEntity, new()
  {
    List<T> items = new();

    await foreach (T item in Stream<T>(null))
    {
      items.Add(item);
    }

    return items;
  }


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


  /// <inheritdoc />
  public virtual string GetChangeToken<T>(T model) where T : ZeroIdEntity, new()
  {
    string changeVector = Session.Advanced.GetChangeVectorFor(model);
    return IdGenerator.HashString(changeVector);
  }
}