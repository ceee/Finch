using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace zero.Stores;

public abstract partial class StoreOperations
{
  /// <inheritdoc />
  public virtual Task<bool> IsAllowedAsChild<T>(T model, string parentId) where T : ZeroIdEntity, IZeroTreeEntity, new()
  {
    return Task.FromResult(true);
  }


  /// <inheritdoc />
  public async Task<Paged<T>> LoadChildren<T>(string parentId, int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default) where T : ZeroIdEntity, IZeroTreeEntity, new()
  {
    IRavenQueryable<T> queryable = Session.Query<T>().Where(x => x.ParentId == parentId).Statistics(out QueryStatistics statistics);
    querySelector ??= x => x.OrderBy(x => x.Sort);

    List<T> result = await querySelector(queryable).Paging(pageNumber, pageSize).ToListAsync();
    return new Paged<T>(result, statistics.TotalResults, pageNumber, pageSize);
  }


  /// <inheritdoc />
  public async Task<Paged<T>> LoadChildren<T, TIndex>(string parentId, int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default) 
    where T : ZeroIdEntity, IZeroTreeEntity, new() 
    where TIndex : AbstractCommonApiForIndexes, new()
  {
    IRavenQueryable<T> queryable = Session.Query<T, TIndex>().Where(x => x.ParentId == parentId).Statistics(out QueryStatistics statistics);
    querySelector ??= x => x.OrderBy(x => x.Sort);

    List<T> result = await querySelector(queryable).Paging(pageNumber, pageSize).ToListAsync();
    return new Paged<T>(result, statistics.TotalResults, pageNumber, pageSize);
  }


  /// <inheritdoc />
  public async Task<EntityResult<IOrderedEnumerable<T>>> Sort<T>(string[] sortedIds) where T : ZeroIdEntity, IZeroTreeEntity, new()
  {
    Dictionary<string, T> items = await Load<T>(sortedIds);
    uint index = 0;

    // contains multiple parents, therefore fail
    if (items.Select(x => x.Value?.ParentId).Distinct().Count() > 1)
    {
      return EntityResult<IOrderedEnumerable<T>>.Fail("@errors.treeentity.sortingmultipleparents");
    }

    foreach (var item in items)
    {
      item.Value.Sort = index;
      index += 10;
      await Update(item.Value);
    }

    return EntityResult<IOrderedEnumerable<T>>.Success(items.Select(x => x.Value).OrderByDescending(x => x.Sort));
  }


  /// <inheritdoc />
  public async Task<EntityResult<T>> Move<T>(string id, string newParentId, Func<T, string, Task<bool>> isParentAllowed = null) where T : ZeroIdEntity, IZeroTreeEntity, new()
  {
    T model = await Load<T>(id);
    T parent = await Load<T>(newParentId);

    if (model == null || (!newParentId.IsNullOrEmpty() && parent == null))
    {
      return EntityResult<T>.Fail("@errors.idnotfound");
    }

    if (isParentAllowed != null && !await isParentAllowed(model, newParentId))
    {
      return EntityResult<T>.Fail("@errors.treeentity.parentnotallowed");
    }

    model.ParentId = parent?.Id;

    return await Update(model);
  }


  /// <inheritdoc />
  public Task<EntityResult<T>> Copy<T>(string id, string newParentId, Func<T, string, Task<bool>> isParentAllowed = null) where T : ZeroIdEntity, IZeroTreeEntity, new() => Copy<T>(id, newParentId, false, isParentAllowed);


  /// <inheritdoc />
  public Task<EntityResult<T>> CopyWithDescendants<T>(string id, string newParentId, Func<T, string, Task<bool>> isParentAllowed = null) where T : ZeroIdEntity, IZeroTreeEntity, new() => Copy<T>(id, newParentId, true, isParentAllowed);


  /// <summary>
  /// Copies an entity (with optional descendants) to a new location
  /// </summary>
  protected async Task<EntityResult<T>> Copy<T>(string id, string newParentId, bool includeDescendants, Func<T, string, Task<bool>> isParentAllowed = null) where T : ZeroIdEntity, IZeroTreeEntity, new()
  {
    T model = await Load<T>(id);
    T parent = await Load<T>(newParentId);

    if (model == null || (!newParentId.IsNullOrEmpty() && parent == null))
    {
      return EntityResult<T>.Fail("@errors.idnotfound");
    }

    string baseId = model.Id;

    // update new page properties
    model.Id = null;
    model.ParentId = parent?.Id;

    if (model is ZeroEntity zeroEntity)
    {
      zeroEntity.IsActive = false;
      zeroEntity.CreatedDate = DateTime.Now;
    }

    // check if new parent is allowed
    if (isParentAllowed != null && !await isParentAllowed(model, newParentId))
    {
      return EntityResult<T>.Fail("@errors.treeentity.parentnotallowed");
    }

    // recursive function to store descendants
    async Task AddChildren(string oldParentId, string newParentId)
    {
      List<T> children = await Session.Query<T>().Where(x => x.ParentId == oldParentId).ToListAsync();

      foreach (T child in children)
      {
        T clonedChild = ObjectCopycat.Clone(child);
        clonedChild.Id = null;
        clonedChild.ParentId = newParentId;
        if (clonedChild is ZeroEntity zeroEntity)
        {
          zeroEntity.IsActive = false;
          zeroEntity.CreatedDate = DateTime.Now;
        }

        await Create(clonedChild);
        await AddChildren(child.Id, clonedChild.Id);
      }
    }

    if (includeDescendants)
    {
      await AddChildren(baseId, model.Id);
    }

    return EntityResult<T>.Success(model);
  }


  /// <inheritdoc />
  public async Task<EntityResult<string[]>> DeleteWithDescendants<T>(T model) where T : ZeroIdEntity, IZeroTreeEntity, new()
  {
    List<T> pages = await GetDescendantsAndSelf(model);

    if (pages.Count < 1)
    {
      return EntityResult<string[]>.Fail("@errors.ondelete.idnotfound");
    }

    await Delete(pages.ToArray());
    return EntityResult<string[]>.Success(pages.Select(x => x.Id).ToArray());
  }


  /// <inheritdoc />
  public async Task<EntityResult<string[]>> DeleteWithDescendants<T>(string id) where T : ZeroIdEntity, IZeroTreeEntity, new() => await DeleteWithDescendants(await Load<T>(id));


  /// <summary>
  /// Get an entity with all its descendants
  /// </summary>
  async Task<List<T>> GetDescendantsAndSelf<T>(T model) where T : ZeroIdEntity, IZeroTreeEntity, new()
  {
    List<T> items = new() { model };

    async Task AddChildren(T parent)
    {
      string parentId = parent.Id;
      List<T> children = await Session.Query<T>().Where(x => x.ParentId == parentId).ToListAsync();
      items.AddRange(children);

      foreach (T child in children)
      {
        await AddChildren(child);
      }
    }

    await AddChildren(model);

    return items;
  }
}