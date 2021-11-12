using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Collections
{
  public abstract partial class CollectionInterceptor<T> : ICollectionInterceptor<T> where T : ZeroEntity
  {
    /// <inheritdoc />
    public virtual bool CanRun(Parameters args) => true;

    /// <inheritdoc />
    public virtual Task<InterceptorResult<T>> Creating(CreateParameters args) => Task.FromResult<InterceptorResult<T>>(default);

    /// <inheritdoc />
    public virtual Task<InterceptorResult<T>> Updating(UpdateParameters args) => Task.FromResult<InterceptorResult<T>>(default);

    /// <inheritdoc />
    public virtual Task<InterceptorResult<T>> Saving(SaveParameters args) => Task.FromResult<InterceptorResult<T>>(default);

    /// <inheritdoc />
    public virtual Task<InterceptorResult<T>> Deleting(DeleteParameters args) => Task.FromResult<InterceptorResult<T>>(default);

    /// <inheritdoc />
    public virtual Task<InterceptorResult<T>> Purging(PurgeParameters args) => Task.FromResult<InterceptorResult<T>>(default);

    /// <inheritdoc />
    public virtual Task Created(CreateParameters args) => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task Updated(UpdateParameters args) => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task Saved(SaveParameters args) => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task Deleted(DeleteParameters args) => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task Purged(PurgeParameters args) => Task.CompletedTask;
  }


  public abstract partial class CollectionInterceptor : CollectionInterceptor<ZeroEntity>, ICollectionInterceptor
  {
    /// <inheritdoc />
    public override bool CanRun(Parameters args) => base.CanRun(args);
  }

  public interface ICollectionInterceptor : ICollectionInterceptor<ZeroEntity> { }

  public interface ICollectionInterceptor<T> where T : ZeroEntity
  {
    /// <summary>
    /// Whether any of the interceptor methods is allowed to run based on the parameters
    /// </summary>
    bool CanRun(CollectionInterceptor<T>.Parameters args);

    /// <summary>
    /// Called after an entity has been stored but before the session has saved its changes
    /// </summary>
    Task Created(CollectionInterceptor<T>.CreateParameters args);

    /// <summary>
    /// Called before an entity is stored and validated
    /// </summary>
    Task<InterceptorResult<T>> Creating(CollectionInterceptor<T>.CreateParameters args);

    /// <summary>
    /// Called after an entity has been deleted but before the session has saved its changes
    /// </summary>
    Task Deleted(CollectionInterceptor<T>.DeleteParameters args);

    /// <summary>
    /// Called before an entity is deleted
    /// </summary>
    Task<InterceptorResult<T>> Deleting(CollectionInterceptor<T>.DeleteParameters args);

    /// <summary>
    /// Called after the document collection has been purged
    /// </summary>
    Task Purged(CollectionInterceptor<T>.PurgeParameters args);

    /// <summary>
    /// Called before a collection is purged
    /// </summary>
    Task<InterceptorResult<T>> Purging(CollectionInterceptor<T>.PurgeParameters args);

    /// <summary>
    /// Called after an entity has been updated but before the session has saved its changes
    /// </summary>
    Task Updated(CollectionInterceptor<T>.UpdateParameters args);

    /// <summary>
    /// Called before an entity is stored and validated
    /// </summary>
    Task<InterceptorResult<T>> Updating(CollectionInterceptor<T>.UpdateParameters args);

    /// <summary>
    /// Called after an entity has been saved (created or updated) but before the session has saved its changes
    /// </summary>
    Task Saved(CollectionInterceptor<T>.SaveParameters args);

    /// <summary>
    /// Called before an entity is stored and validated
    /// </summary>
    Task<InterceptorResult<T>> Saving(CollectionInterceptor<T>.SaveParameters args);
  }
}
