using FluentValidation.Results;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;

namespace zero.Stores;

public abstract class EntityStore<T> : IEntityStore<T> where T : ZeroIdEntity, ISupportsFlavors, ISupportsSorting, new()
{
  /// <inheritdoc />
  public Guid Guid { get; private set; } = Guid.NewGuid();

  /// <inheritdoc />
  public IZeroDocumentSession Session => Operations.Session;

  /// <inheritdoc />
  public StoreConfig Config => Operations.Config;

  protected IZeroContext Context { get; private set; }


  protected IInterceptors Interceptors { get; private set; }

  protected IStoreOperations Operations { get; private set; }

  protected IZeroOptions Options { get; private set; }


  public EntityStore(IStoreContext collectionContext)
  {
    Context = collectionContext.Context;
    Interceptors = collectionContext.Interceptors;
    Options = collectionContext.Options;
    Operations = new StoreOperations(collectionContext);
  }


  /// <inheritdoc />
  public virtual Task<T> Empty(string flavorAlias = null) => Operations.Empty<T>(flavorAlias);

  /// <inheritdoc />
  public virtual Task<TFlavor> Empty<TFlavor>(string flavorAlias = null) where TFlavor : T, new() => Operations.Empty<T, TFlavor>(flavorAlias);

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
  public virtual IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IQueryable<T>> expression) => Operations.Stream<T>(expression);

  /// <inheritdoc />
  public virtual string GetChangeToken(T model) => Operations.GetChangeToken(model);

  /// <inheritdoc />
  public virtual Task<Result<T>> Create(T model) => Operations.Create(model, async (m, ctx) => await Validate(ctx, m));

  /// <inheritdoc />
  public virtual Task<Result<T>> Update(T model) => Operations.Update(model, async (m, ctx) => await Validate(ctx, m));

  /// <inheritdoc />
  public virtual Task<Result<IOrderedEnumerable<T>>> Sort(string[] sortedIds) => Operations.Sort<T>(sortedIds);

  /// <inheritdoc />
  public virtual Task<Result<T>> Delete(T model) => Operations.Delete(model);

  /// <inheritdoc />
  public virtual async Task<ValidationResult> Validate(ZeroValidationContext ctx, T model)
  {
    ZeroValidator<T> validator = new();
    ValidationRules(validator);
    return await validator.ValidateAsync(model);
  }

  /// <inheritdoc />
  public Task<ValidationResult> Validate(T model) => Validate(new ZeroValidationContext()
  {
    Context = Context,
    Session = Session,
    Operation = ValidationOp.Unknown
  }, model);


  /// <summary>
  /// Create rules for validation
  /// </summary>
  protected virtual void ValidationRules(ZeroValidator<T> validator) { }


  /// <summary>
  /// Do only return the model when it is set to active or inactive entities are included with IncludeInactive()
  /// </summary>
  protected virtual T WhenActive(T model) => model != null && (Config.IncludeInactive || (model is not ZeroEntity || (model as ZeroEntity).IsActive)) ? model : default;
}



public interface IEntityStore<T> where T : ZeroIdEntity, ISupportsFlavors, ISupportsSorting, new()
{
  /// <summary>
  /// Id for this store
  /// </summary>
  Guid Guid { get; }

  /// <summary>
  /// Access the current document session
  /// </summary>
  IZeroDocumentSession Session { get; }

  /// <summary>
  /// Configure the store
  /// </summary>
  StoreConfig Config { get; }

  /// <summary>
  /// Get new instance of an entity (with an optional flavor)
  /// </summary>
  Task<T> Empty(string flavorAlias = null);

  /// <summary>
  /// Get new instance of an entity with a specific flavor
  /// </summary>
  Task<TFlavor> Empty<TFlavor>(string flavorAlias = null) where TFlavor : T, new();

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
  IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IQueryable<T>> expression);

  /// <summary>
  /// Get the change vector for a model (Proxy to IAsyncDocumentSession.GetChangeVectorFor<>)
  /// </summary>
  string GetChangeToken(T model);

  /// <summary>
  /// Validates an entity in this collection
  /// </summary>
  Task<ValidationResult> Validate(ZeroValidationContext ctx, T model);

  /// <summary>
  /// Creates an entity with an optional validator
  /// </summary>
  Task<Result<T>> Create(T model);

  /// <summary>
  /// Updates an entity with an optional validator
  /// </summary>
  Task<Result<T>> Update(T model);

  /// <summary>
  /// Update sorting of entities on a specific level
  /// </summary>
  Task<Result<IOrderedEnumerable<T>>> Sort(string[] sortedIds);

  /// <summary>
  /// Deletes an entity
  /// </summary>
  Task<Result<T>> Delete(T model);
}