using FluentValidation.Results;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;

namespace zero.Stores;

public abstract class EntityStore<T> : IEntityStore<T> where T : ZeroIdEntity, new()
{
  /// <inheritdoc />
  public Guid Guid { get; private set; } = Guid.NewGuid();

  /// <inheritdoc />
  public IZeroDocumentSession Session => Context.Store.Session();


  protected record EntityCollectionOptions(bool IncludeInactive);

  protected IZeroContext Context { get; private set; }

  protected EntityCollectionOptions Options { get; set; }

  protected IInterceptors Interceptors { get; private set; }

  protected IStoreOperations Operations { get; private set; }


  public EntityStore(IStoreContext collectionContext)
  {
    Operations = collectionContext.Operations;
    Context = collectionContext.Context;
    Interceptors = collectionContext.Interceptors;
    Options = new(IncludeInactive: true);
  }


  /// <inheritdoc />
  public virtual Task<T> Empty() => Operations.Empty<T>();

  /// <inheritdoc />
  public virtual Task<T> Load(string id, string changeVector = null) => Operations.Load<T>(id, changeVector);

  /// <inheritdoc />
  public virtual Task<Dictionary<string, T>> Load(IEnumerable<string> ids) => Operations.Load<T>(ids);

  /// <inheritdoc />
  public virtual Task<Paged<T>> Load(int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default) => Operations.Load(pageNumber, pageSize, querySelector);

  /// <inheritdoc />
  public virtual Task<Paged<T>> Load<TIndex>(int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default) where TIndex : AbstractCommonApiForIndexes, new() => Operations.Load<T, TIndex>(pageNumber, pageSize, querySelector);

  /// <inheritdoc />
  public virtual Task<List<T>> LoadAll() => Operations.LoadAll<T>();

  /// <inheritdoc />
  public virtual IAsyncEnumerable<T> Stream() => Operations.Stream<T>();

  /// <inheritdoc />
  public virtual IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IQueryable<T>> expression) => Operations.Stream<T>(expression);

  /// <inheritdoc />
  public virtual Task<EntityResult<T>> Create(T model) => Operations.Create(model, async m => await Validate(m));

  /// <inheritdoc />
  public virtual Task<EntityResult<T>> Update(T model) => Operations.Update(model, async m => await Validate(m));

  /// <inheritdoc />
  public virtual Task<EntityResult<T>> Delete(T model) => Operations.Delete(model);

  /// <inheritdoc />
  public virtual Task<int> Delete(IEnumerable<T> models) => Operations.Delete(models);

  /// <inheritdoc />
  public virtual Task<EntityResult<T>> Delete(string id) => Operations.Delete<T>(id);

  /// <inheritdoc />
  public virtual Task<int> Delete(IEnumerable<string> ids) => Operations.Delete<T>(ids);

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
  protected virtual T WhenActive(T model) => model != null && (Options.IncludeInactive || (model is ZeroEntity ? (model as ZeroEntity).IsActive : true)) ? model : default;
}



public interface IEntityStore<T> where T : ZeroIdEntity, new()
{
  /// <summary>
  /// Get new instance of an entity
  /// </summary>
  Task<T> Empty();

  /// <summary>
  /// Get an entity by Id
  /// </summary>
  Task<T> Load(string id, string changeVector = null);

  /// <summary>
  /// Get entities by ids
  /// </summary>
  Task<Dictionary<string, T>> Load(IEnumerable<string> ids);

  /// <summary>
  /// Get entities by query
  /// </summary>
  Task<Paged<T>> Load(int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default);

  /// <summary>
  /// Get entities by query (by using the specified index)
  /// </summary>
  Task<Paged<T>> Load<TIndex>(int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default) where TIndex : AbstractCommonApiForIndexes, new();

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
  IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IQueryable<T>> expression);

  /// <summary>
  /// Validates an entity in this collection
  /// </summary>
  Task<ValidationResult> Validate(T model);

  /// <summary>
  /// Creates an entity with an optional validator
  /// </summary>
  Task<EntityResult<T>> Create(T model);

  /// <summary>
  /// Updates an entity with an optional validator
  /// </summary>
  Task<EntityResult<T>> Update(T model);

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