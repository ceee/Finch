using FluentValidation;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;

namespace zero.Core.Backoffice
{
  public abstract class BackofficeServiceInterceptor : IBackofficeServiceInterceptor
  {
    /// <inheritdoc />
    public virtual HashSet<Type> Types { get; } = new();

    /// <inheritdoc />
    public virtual Task<EntityResult<T>> Creating<T>(CreateParameters<T> args) where T : IZeroEntity => Task.FromResult<EntityResult<T>>(default);

    /// <inheritdoc />
    public virtual Task<EntityResult<T>> Updating<T>(UpdateParameters<T> args) where T : IZeroEntity => Task.FromResult<EntityResult<T>>(default);

    /// <inheritdoc />
    public virtual Task<EntityResult<T>> Deleting<T>(DeleteParameters<T> args) where T : IZeroEntity => Task.FromResult<EntityResult<T>>(default);

    /// <inheritdoc />
    public virtual Task<EntityResult<T>> Purging<T>(PurgeParameters<T> args) where T : IZeroEntity => Task.FromResult<EntityResult<T>>(default);

    /// <inheritdoc />
    public virtual Task Created<T>(CreateParameters<T> args) where T : IZeroEntity => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task Updated<T>(UpdateParameters<T> args) where T : IZeroEntity => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task Deleted<T>(DeleteParameters<T> args) where T : IZeroEntity => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task Purged<T>(PurgeParameters<T> args) where T : IZeroEntity => Task.CompletedTask;


    public class Parameters<T> where T : IZeroEntity
    {
      /// <summary>
      /// The current zero context
      /// </summary>
      public IZeroContext Context { get; set; }

      /// <summary>
      /// Raven document store
      /// </summary>
      public IZeroStore Store { get; set; }

      /// <summary>
      /// Currently loaded document session
      /// </summary>
      public IAsyncDocumentSession Session { get; set; }

      /// <summary>
      /// Validator for the affected entity
      /// </summary>
      public IValidator<T> Validator { get; set; }
    }

    public class CreateParameters<T> : Parameters<T> where T : IZeroEntity
    {
      /// <summary>
      /// The model which is created
      /// </summary>
      public T Model { get; set; }
    }

    public class UpdateParameters<T> : Parameters<T> where T : IZeroEntity
    {
      /// <summary>
      /// The Id of the model which is updated
      /// </summary>
      public string Id { get; set; }

      /// <summary>
      /// The module which is updated
      /// </summary>
      public T Model { get; set; }
    }

    public class DeleteParameters<T> : Parameters<T> where T : IZeroEntity
    {
      /// <summary>
      /// The id of the model which is deleted
      /// </summary>
      public string Id { get; set; }

      /// <summary>
      /// The model which is deleted
      /// </summary>
      public T Model { get; set; }
    }

    public class PurgeParameters<T> : Parameters<T> where T : IZeroEntity { }
  }


  public interface IBackofficeServiceInterceptor
  {
    /// <summary>
    /// Affacted document types
    /// </summary>
    HashSet<Type> Types { get; }

    /// <summary>
    /// Called after an entity has been stored but before the session has saved its changes
    /// </summary>
    Task Created<T>(BackofficeServiceInterceptor.CreateParameters<T> args) where T : IZeroEntity;

    /// <summary>
    /// Called before an entity is stored and validated
    /// </summary>
    Task<EntityResult<T>> Creating<T>(BackofficeServiceInterceptor.CreateParameters<T> args) where T : IZeroEntity;

    /// <summary>
    /// Called after an entity has been deleted but before the session has saved its changes
    /// </summary>
    Task Deleted<T>(BackofficeServiceInterceptor.DeleteParameters<T> args) where T : IZeroEntity;

    /// <summary>
    /// Called before an entity is deleted
    /// </summary>
    Task<EntityResult<T>> Deleting<T>(BackofficeServiceInterceptor.DeleteParameters<T> args) where T : IZeroEntity;

    /// <summary>
    /// Called after the document collection has been purged
    /// </summary>
    Task Purged<T>(BackofficeServiceInterceptor.PurgeParameters<T> args) where T : IZeroEntity;

    /// <summary>
    /// Called before a collection is purged
    /// </summary>
    Task<EntityResult<T>> Purging<T>(BackofficeServiceInterceptor.PurgeParameters<T> args) where T : IZeroEntity;

    /// <summary>
    /// Called after an entity has been updated but before the session has saved its changes
    /// </summary>
    Task Updated<T>(BackofficeServiceInterceptor.UpdateParameters<T> args) where T : IZeroEntity;

    /// <summary>
    /// Called before an entity is stored and validated
    /// </summary>
    Task<EntityResult<T>> Updating<T>(BackofficeServiceInterceptor.UpdateParameters<T> args) where T : IZeroEntity;
  }
}
