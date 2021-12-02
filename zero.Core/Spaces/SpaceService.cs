using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace zero.Spaces;

public class SpaceService : SpaceStore, ISpaceService
{
  /// <inheritdoc />
  public ISpaceTypeService Types { get; protected set; }


  public SpaceService(IStoreContext store, ISpaceTypeService spaceTypeService) : base(store)
  {
    Types = spaceTypeService;
  }


  /// <inheritdoc />
  public async Task<T> Load<T>(string id, string changeVector = null) where T : Space, new()
  {
    return await Load(id, changeVector) as T;
  }


  /// <inheritdoc />
  public async Task<Dictionary<string, T>> Load<T>(string spaceType, IEnumerable<string> ids) where T : Space, new()
  {
    ids = ids.Distinct().ToArray();

    Dictionary<string, T> models = await Session.LoadAsync<T>(ids);
    Dictionary<string, T> result = new();

    foreach (string id in ids)
    {
      models.TryGetValue(id, out T model);

      if (!typeof(T).IsAssignableFrom(model.GetType()))
      {
        result.Add(id, null);
      }
      else
      {
        result.Add(id, WhenActive(model) as T);
      }
    }

    return result;
  }


  /// <inheritdoc />
  public async Task<Paged<T>> Load<T>(string spaceType, int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> expression = null) where T : Space, new()
  {
    IRavenQueryable<T> queryable = Session.Query<T>().Where(x => x.Flavor == spaceType).Statistics(out QueryStatistics statistics);
    expression ??= x => x;

    List<T> result = await expression(queryable).Paging(pageNumber, pageSize).ToListAsync();
    return new Paged<T>(result, statistics.TotalResults, pageNumber, pageSize);
  }


  /// <inheritdoc />
  public async Task<Paged<T>> Load<T, TIndex>(string spaceType, int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> expression = null)
    where T : Space, new()
    where TIndex : AbstractCommonApiForIndexes, new()
  {
    IRavenQueryable<T> queryable = Session.Query<T, TIndex>().Where(x => x.Flavor == spaceType).Statistics(out QueryStatistics statistics);
    expression ??= x => x;

    List<T> result = await expression(queryable).Paging(pageNumber, pageSize).ToListAsync();
    return new Paged<T>(result, statistics.TotalResults, pageNumber, pageSize);
  }
}


public interface ISpaceService : ISpaceStore
{
  /// <summary>
  /// Service for retrieving space types
  /// </summary>
  ISpaceTypeService Types { get; }

  /// <summary>
  /// Get editor item for a space
  /// </summary>
  Task<T> Load<T>(string id, string changeVector = null) where T : Space, new();

  /// <summary>
  /// Get editor items for a space
  /// </summary>
  Task<Dictionary<string, T>> Load<T>(string spaceType, IEnumerable<string> ids) where T : Space, new();

  /// <summary>
  /// Get space items by query
  /// </summary>
  Task<Paged<T>> Load<T>(string spaceType, int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> expression = default) where T : Space, new();

  /// <summary>
  /// Get space items by query (by using the specified index)
  /// </summary>
  Task<Paged<T>> Load<T, TIndex>(string spaceType, int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> expression = default) where T : Space, new() where TIndex : AbstractCommonApiForIndexes, new();
}