namespace zero.Communication;

public abstract partial class CollectionInterceptor<T> : ICollectionInterceptor<T> where T : ZeroIdEntity
{
  /// <inheritdoc />
  public virtual bool CanRun(InterceptorParameters args, T model) => true;

  /// <inheritdoc />
  public virtual Task<InterceptorResult<T>> Creating(InterceptorParameters args, T model) => Task.FromResult<InterceptorResult<T>>(default);

  /// <inheritdoc />
  public virtual Task<InterceptorResult<T>> Updating(InterceptorParameters args, T model) => Task.FromResult<InterceptorResult<T>>(default);

  /// <inheritdoc />
  public virtual Task<InterceptorResult<T>> Saving(InterceptorParameters args, T model) => Task.FromResult<InterceptorResult<T>>(default);

  /// <inheritdoc />
  public virtual Task<InterceptorResult<T>> Deleting(InterceptorParameters args, T model) => Task.FromResult<InterceptorResult<T>>(default);

  /// <inheritdoc />
  public virtual Task Created(InterceptorParameters args, T model) => Task.CompletedTask;

  /// <inheritdoc />
  public virtual Task Updated(InterceptorParameters args, T model) => Task.CompletedTask;

  /// <inheritdoc />
  public virtual Task Saved(InterceptorParameters args, T model) => Task.CompletedTask;

  /// <inheritdoc />
  public virtual Task Deleted(InterceptorParameters args, T model) => Task.CompletedTask;
}


public abstract partial class CollectionInterceptor : CollectionInterceptor<ZeroIdEntity>, ICollectionInterceptor
{
  /// <inheritdoc />
  public override bool CanRun(InterceptorParameters args, ZeroIdEntity model) => base.CanRun(args, model);
}

public interface ICollectionInterceptor : ICollectionInterceptor<ZeroIdEntity> { }

public interface ICollectionInterceptor<T> where T : ZeroIdEntity
{
  /// <summary>
  /// Whether any of the interceptor methods is allowed to run based on the parameters
  /// </summary>
  bool CanRun(InterceptorParameters args, T model);

  /// <summary>
  /// Called after an entity has been stored but before the session has saved its changes
  /// </summary>
  Task Created(InterceptorParameters args, T model);

  /// <summary>
  /// Called before an entity is stored and validated
  /// </summary>
  Task<InterceptorResult<T>> Creating(InterceptorParameters args, T model);

  /// <summary>
  /// Called after an entity has been deleted but before the session has saved its changes
  /// </summary>
  Task Deleted(InterceptorParameters args, T model);

  /// <summary>
  /// Called before an entity is deleted
  /// </summary>
  Task<InterceptorResult<T>> Deleting(InterceptorParameters args, T model);

  /// <summary>
  /// Called after an entity has been updated but before the session has saved its changes
  /// </summary>
  Task Updated(InterceptorParameters args, T model);

  /// <summary>
  /// Called before an entity is stored and validated
  /// </summary>
  Task<InterceptorResult<T>> Updating(InterceptorParameters args, T model);

  /// <summary>
  /// Called after an entity has been saved (created or updated) but before the session has saved its changes
  /// </summary>
  Task Saved(InterceptorParameters args, T model);

  /// <summary>
  /// Called before an entity is stored and validated
  /// </summary>
  Task<InterceptorResult<T>> Saving(InterceptorParameters args, T model);
}