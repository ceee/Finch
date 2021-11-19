namespace zero.Core;
using FluentValidation.Results;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Validation;


public abstract partial class EntityCollection<T> : IEntityCollection<T> where T : ZeroIdEntity
{
  /// <inheritdoc />
  public Guid Guid { get; private set; } = Guid.NewGuid();

  /// <inheritdoc />
  public IZeroDocumentSession Session => Context.Store.Session();


  protected record EntityCollectionOptions(bool IncludeInactive);

  protected IZeroContext Context { get; private set; }

  protected EntityCollectionOptions Options { get; private set; }

  protected InterceptorRunner<T> Interceptors { get; private set; }


  public EntityCollection()
  {
    Options = new(true);
  }


  /// <inheritdoc />
  public virtual async Task<ValidationResult> Validate(T model)
  {
    ZeroValidator<T> validator = new();
    ValidationRules(validator);
    return await validator.ValidateAsync(model);
  }


  /// <summary>
  /// Create rules for validation
  /// </summary>
  protected virtual void ValidationRules(ZeroValidator<T> validator) { }


  /// <summary>
  /// Do only return the model when it is set to active or inactive entities are included with IncludeInactive()
  /// </summary>
  protected virtual T WhenActive(T model)
  {
    return model != null && (Options.IncludeInactive || (model is ZeroEntity ? (model as ZeroEntity).IsActive : true)) ? model : default;
  }
}



public interface IEntityCollection<T> where T : ZeroIdEntity
{
  /// <summary>
  /// Get an entity by Id
  /// </summary>
  Task<T> Load(string id, string changeVector = null);

  /// <summary>
  /// Get entities by ids
  /// </summary>
  Task<Dictionary<string, T>> Load(params string[] ids);

  /// <summary>
  /// Get entities by query
  /// </summary>
  Task<ListResult<T>> Load(ListQuery<T> query);

  /// <summary>
  /// Get entities by query (by using the specified index)
  /// </summary>
  Task<ListResult<T>> Load<TIndex>(ListQuery<T> query) where TIndex : AbstractCommonApiForIndexes, new();

  /// <summary>
  /// Get all entities from this collection. 
  /// Warning: Don't use this method for large collections. Stream the results instead.
  /// </summary>
  Task<List<T>> LoadAll();

  /// <summary>
  /// Stream the collection
  /// </summary>
  IAsyncEnumerable<T> Stream();

  /// <summary>
  /// Stream the collection
  /// </summary>
  IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IRavenQueryable<T>> expression);

  /// <summary>
  /// Validates an entity in this collection
  /// </summary>
  Task<ValidationResult> Validate(T model);

  /// <summary>
  /// Updates or creates an entity with an optional validator
  /// </summary>
  Task<EntityResult<T>> Save(T model);

  /// <summary>
  /// Deletes an entity
  /// </summary>
  Task<EntityResult<T>> Delete(T model);

  /// <summary>
  /// Deletes entities
  /// </summary>
  Task<int> Delete(IEnumerable<T> models);

  /// <summary>
  /// Deletes an entity by Id
  /// </summary>
  Task<EntityResult<T>> Delete(string id);

  /// <summary>
  /// Deletes entities by Id
  /// </summary>
  Task<int> Delete(IEnumerable<string> ids);
}