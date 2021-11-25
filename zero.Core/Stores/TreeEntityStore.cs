using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;

namespace zero.Stores;

public abstract class TreeEntityStore<T> : EntityStore<T>, ITreeEntityStore<T> where T : ZeroIdEntity, IZeroTreeEntity, new()
{
  public TreeEntityStore(IStoreContext collectionContext) : base(collectionContext) { }


  /// <inheritdoc />
  public override async Task<EntityResult<T>> Create(T model)
  {
    if (!await IsAllowedAsChild(model, model.ParentId))
    {
      return EntityResult<T>.Fail("@errors.treeentity.parentnotallowed");
    }
    return await base.Create(model);
  }

  /// <inheritdoc />
  public override async Task<EntityResult<T>> Update(T model)
  {
    if (!await IsAllowedAsChild(model, model.ParentId))
    {
      return EntityResult<T>.Fail("@errors.treeentity.parentnotallowed");
    }
    return await base.Update(model);
  }

  /// <inheritdoc />
  public virtual Task<bool> IsAllowedAsChild(T model, string parentId) => Task.FromResult(true);

  /// <inheritdoc />
  public virtual Task<EntityResult<T>> Copy(string id, string newParentId) => Operations.Copy<T>(id, newParentId, async (model, parentId) => await IsAllowedAsChild(model, parentId));

  /// <inheritdoc />
  public virtual Task<EntityResult<T>> CopyWithDescendants(string id, string newParentId) => Operations.CopyWithDescendants<T>(id, newParentId, async (model, parentId) => await IsAllowedAsChild(model, parentId));

  /// <inheritdoc />
  public virtual Task<EntityResult<T>> Move(string id, string newParentId) => Operations.Move<T>(id, newParentId, async (model, parentId) => await IsAllowedAsChild(model, parentId));

  /// <inheritdoc />
  public virtual Task<EntityResult<IOrderedEnumerable<T>>> Sort(string[] sortedIds) => Operations.Sort<T>(sortedIds);

  /// <inheritdoc />
  public virtual Task<EntityResult<string[]>> DeleteWithDescendants(T model) => Operations.DeleteWithDescendants(model);

  /// <inheritdoc />
  public virtual Task<EntityResult<string[]>> DeleteWithDescendants(string id) => Operations.DeleteWithDescendants<T>(id);

  /// <inheritdoc />
  public virtual Task<Paged<T>> LoadChildren(string parentId, int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = null) 
    => Operations.LoadChildren(parentId, pageNumber, pageSize, querySelector);

  /// <inheritdoc />
  public virtual Task<Paged<T>> LoadChildren<TIndex>(string parentId, int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = null) where TIndex : AbstractCommonApiForIndexes, new() 
    => Operations.LoadChildren<T, TIndex>(parentId, pageNumber, pageSize, querySelector);
}



public interface ITreeEntityStore<T> : IEntityStore<T> where T : ZeroIdEntity, IZeroTreeEntity, new()
{
  /// <summary>
  /// Determines whether a model is allowed as a child for a new parent.
  /// This is primarily used by copy/move operations
  /// </summary>
  Task<bool> IsAllowedAsChild(T model, string parentId);

  /// <summary>
  /// Loads all children for an entity
  /// </summary>
  Task<Paged<T>> LoadChildren(string parentId, int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default);

  /// <summary>
  /// Get descendants by query (by using the specified index)
  /// </summary>
  Task<Paged<T>> LoadChildren<TIndex>(string parentId, int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default) where TIndex : AbstractCommonApiForIndexes, new();

  /// <summary>
  /// Update sorting of entities on a specific level
  /// </summary>
  Task<EntityResult<IOrderedEnumerable<T>>> Sort(string[] sortedIds);

  /// <summary>
  /// Move an entity to a new parent
  /// </summary>
  Task<EntityResult<T>> Move(string id, string newParentId);

  /// <summary>
  /// Copies an entity to a new location
  /// </summary>
  Task<EntityResult<T>> Copy(string id, string newParentId);

  /// <summary>
  /// Copies an entity with descendants to a new location
  /// </summary>
  Task<EntityResult<T>> CopyWithDescendants(string id, string newParentId);

  /// <summary>
  /// Deletes an entity with all descendents
  /// </summary>
  Task<EntityResult<string[]>> DeleteWithDescendants(T model);

  /// <summary>
  /// Deletes an entity by Id with all descendents
  /// </summary>
  Task<EntityResult<string[]>> DeleteWithDescendants(string id);
}