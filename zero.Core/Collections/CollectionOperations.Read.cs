using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;

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
  public virtual async Task<ListResult<T>> Load<T>(ListQuery<T> query) where T : ZeroIdEntity, new()
  {
    return await Session.Query<T>().FilterAsync(query);
  }


  /// <inheritdoc />
  public virtual async Task<ListResult<T>> Load<T, TIndex>(ListQuery<T> query) 
    where T : ZeroIdEntity, new() 
    where TIndex : AbstractCommonApiForIndexes, new()
  {
    return await Session.Query<T, TIndex>().FilterAsync(query);
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
  public virtual async IAsyncEnumerable<T> Stream<T>(Func<IRavenQueryable<T>, IRavenQueryable<T>> expression) where T : ZeroIdEntity, new()
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