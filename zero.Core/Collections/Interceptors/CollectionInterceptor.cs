using System;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Collections
{
  public abstract partial class CollectionInterceptor : ICollectionInterceptor
  {
    /// <inheritdoc />
    public virtual Task<InterceptorResult<T>> Creating<T>(CreateParameters<T> args) where T : ZeroEntity => Task.FromResult<InterceptorResult<T>>(default);

    /// <inheritdoc />
    public virtual Task<InterceptorResult<T>> Updating<T>(UpdateParameters<T> args) where T : ZeroEntity => Task.FromResult<InterceptorResult<T>>(default);

    /// <inheritdoc />
    public virtual Task<InterceptorResult<T>> Deleting<T>(DeleteParameters<T> args) where T : ZeroEntity => Task.FromResult<InterceptorResult<T>>(default);

    /// <inheritdoc />
    public virtual Task<InterceptorResult<T>> Purging<T>(PurgeParameters<T> args) where T : ZeroEntity => Task.FromResult<InterceptorResult<T>>(default);

    /// <inheritdoc />
    public virtual Task Created<T>(CreateParameters<T> args) where T : ZeroEntity => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task Updated<T>(UpdateParameters<T> args) where T : ZeroEntity => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task Deleted<T>(DeleteParameters<T> args) where T : ZeroEntity => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task Purged<T>(PurgeParameters<T> args) where T : ZeroEntity => Task.CompletedTask;
  }


  public interface ICollectionInterceptor
  {
    /// <summary>
    /// Called after an entity has been stored but before the session has saved its changes
    /// </summary>
    Task Created<T>(CollectionInterceptor.CreateParameters<T> args) where T : ZeroEntity;

    /// <summary>
    /// Called before an entity is stored and validated
    /// </summary>
    Task<InterceptorResult<T>> Creating<T>(CollectionInterceptor.CreateParameters<T> args) where T : ZeroEntity;

    /// <summary>
    /// Called after an entity has been deleted but before the session has saved its changes
    /// </summary>
    Task Deleted<T>(CollectionInterceptor.DeleteParameters<T> args) where T : ZeroEntity;

    /// <summary>
    /// Called before an entity is deleted
    /// </summary>
    Task<InterceptorResult<T>> Deleting<T>(CollectionInterceptor.DeleteParameters<T> args) where T : ZeroEntity;

    /// <summary>
    /// Called after the document collection has been purged
    /// </summary>
    Task Purged<T>(CollectionInterceptor.PurgeParameters<T> args) where T : ZeroEntity;

    /// <summary>
    /// Called before a collection is purged
    /// </summary>
    Task<InterceptorResult<T>> Purging<T>(CollectionInterceptor.PurgeParameters<T> args) where T : ZeroEntity;

    /// <summary>
    /// Called after an entity has been updated but before the session has saved its changes
    /// </summary>
    Task Updated<T>(CollectionInterceptor.UpdateParameters<T> args) where T : ZeroEntity;

    /// <summary>
    /// Called before an entity is stored and validated
    /// </summary>
    Task<InterceptorResult<T>> Updating<T>(CollectionInterceptor.UpdateParameters<T> args) where T : ZeroEntity;
  }
}
