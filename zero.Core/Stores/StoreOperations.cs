using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using System.Security.Claims;

namespace zero.Stores;

public partial class StoreOperations : 
  IStoreOperations,
  ISharedStoreOperations,
  IStoreOperationsWithInactive,
  ISharedStoreOperationsWithInactive
{
  /// <inheritdoc />
  public IZeroDocumentSession Session => Context.Store.Session(_overrideDatabase ?? Config.Database);

  /// <inheritdoc />
  public StoreConfig Config { get; set; }

  protected record OperationOptions(bool IncludeInactive);

  protected IZeroContext Context { get; private set; }

  protected IInterceptors Interceptors { get; private set; }

  protected FlavorOptions Flavors { get; private set; }

  protected IServiceProvider Services { get; private set; }

  string _overrideDatabase = null;


  public StoreOperations(IStoreContext context, StoreConfig config = null)
  {
    Context = context.Context;
    Interceptors =  context.Interceptors;
    Services = context.Services;
    Flavors = context.Options.For<FlavorOptions>();
    Config = config ?? new();
  }


  /// <inheritdoc />
  public async Task<string> GenerateId<T>(T model) where T : ZeroIdEntity
  {
    IZeroDocumentSession session = Session;
    return await session.Advanced.DocumentStore.Conventions.GenerateDocumentIdAsync(session.Advanced.DocumentStore.Database, model);
  }


  /// <inheritdoc />
  public T AutoSetIds<T>(T model)
  {
    return IdGenerator.Autofill(model);
  }


  /// <inheritdoc />
  public T PrepareForSave<T>(T model) where T : ZeroIdEntity
  {
    // set IDs
    AutoSetIds(model);

    if (model is ZeroEntity zeroModel)
    {
      // get current user
      string userId = Context.BackofficeUser.FindFirstValue(Constants.Auth.Claims.UserId);

      // set default properties
      if (zeroModel.CreatedDate == default)
      {
        zeroModel.CreatedDate = DateTimeOffset.Now;
      }
      if (zeroModel.CreatedById.IsNullOrEmpty())
      {
        zeroModel.CreatedById = userId;
      }
      if (zeroModel.LanguageId.IsNullOrEmpty())
      {
        zeroModel.LanguageId ??= "languages.1-A"; // TODO correct language id
      }

      // update name alias and last modified
      zeroModel.Alias = Safenames.Alias(zeroModel.Name);
      zeroModel.LastModifiedById = userId;
      zeroModel.LastModifiedDate = DateTimeOffset.Now;
      zeroModel.CreatedById ??= userId;
      zeroModel.Hash ??= IdGenerator.Create();
    }

    if (model is IAlwaysActive activeModel)
    {
      activeModel.IsActive = true;
    }

    return model;
  }


  /// <summary>
  /// Validates an entity
  /// </summary>
  public async Task<ValidationResult> Validate<T>(T model) where T : ZeroIdEntity, new()
  {
    IZeroMergedValidator<T> validator = Services.GetService<IZeroMergedValidator<T>>();

    if (validator == null)
    {
      return new();
    }

    return await validator.ValidateAsync(model);
  }


  /// <summary>
  /// Do only return the model when it is set to active or inactive entities are included with IncludeInactive()
  /// </summary>
  protected virtual T WhenActive<T>(T model) where T : ZeroIdEntity, new()
  {
    return model != null && (Config.IncludeInactive || model is IAlwaysActive || model is not ZeroEntity || (model as ZeroEntity).IsActive) ? model : default;
  }
}



public interface IStoreOperations
{
  /// <summary>
  /// Access to the current session
  /// </summary>
  IZeroDocumentSession Session { get; }

  /// <summary>
  /// Configure operations
  /// </summary>
  StoreConfig Config { get; set; }

  /// <summary>
  /// Get new instance of an entity (with an optional flavor)
  /// </summary>
  Task<T> Empty<T>(string flavorAlias = null) where T : ZeroIdEntity, ISupportsFlavors, new();

  /// <summary>
  /// Get new instance of an entity with a specific flavor
  /// </summary>
  /// <param name="flavorAlias">Optional alias. If left out the default flavor is used (if configured)</param>
  Task<TFlavor> Empty<T, TFlavor>(string flavorAlias = null)
    where T : ZeroIdEntity, ISupportsFlavors, new()
    where TFlavor : T, new();

  /// <summary>
  /// Generate model Id by using configured document store conventions
  /// </summary>
  Task<string> GenerateId<T>(T model) where T : ZeroIdEntity;

  /// <summary>
  /// Generate values for all properties (incl. nested) which contain the [GenerateId] attribute
  /// </summary>
  T AutoSetIds<T>(T model);

  /// <summary>
  /// Automatically fill base properties of a ZeroEntity
  /// </summary>
  T PrepareForSave<T>(T model) where T : ZeroIdEntity;

  /// <summary>
  /// Check if any items exist in this collection (with optional query)
  /// </summary>
  Task<bool> Any<T>(Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default) where T : ZeroIdEntity, new();

  /// <summary>
  /// Get an entity by Id
  /// </summary>
  Task<T> Load<T>(string id, string changeVector = null) where T : ZeroIdEntity, new();

  /// <summary>
  /// Get entities by ids
  /// </summary>
  Task<Dictionary<string, T>> Load<T>(IEnumerable<string> ids) where T : ZeroIdEntity, new();

  /// <summary>
  /// Get entities by query
  /// </summary>
  Task<Paged<T>> Load<T>(int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> expression = default) where T : ZeroIdEntity, new();

  /// <summary>
  /// Get entities by query (by using the specified index)
  /// </summary>
  Task<Paged<T>> Load<T, TIndex>(int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> expression = default) where T : ZeroIdEntity, new() where TIndex : AbstractCommonApiForIndexes, new();

  /// <summary>
  /// Get all entities from this collection. 
  /// Warning: Don't use this method for large collections. Stream the results instead.
  /// </summary>
  Task<List<T>> LoadAll<T>() where T : ZeroIdEntity, new();

  /// <summary>
  /// Stream the collection
  /// </summary>
  IAsyncEnumerable<T> Stream<T>(Func<IRavenQueryable<T>, IQueryable<T>> expression) where T : ZeroIdEntity, new();

  /// <summary>
  /// Get the change vector for a model
  /// </summary>
  string GetChangeToken<T>(T model) where T : ZeroIdEntity, new();

  /// <summary>
  /// Validates an entity
  /// </summary>
  Task<ValidationResult> Validate<T>(T model) where T : ZeroIdEntity, new();

  /// <summary>
  /// Creates an entity with an optional validator
  /// </summary>
  Task<Result<T>> Create<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new();

  /// <summary>
  /// Updates an entity with an optional validator
  /// </summary>
  Task<Result<T>> Update<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new();

  /// <inheritdoc />
  Task<Result<IOrderedEnumerable<T>>> Sort<T>(string[] sortedIds) where T : ZeroIdEntity, ISupportsSorting, new();

  /// <summary>
  /// Deletes an entity
  /// </summary>
  Task<Result<T>> Delete<T>(T model) where T : ZeroIdEntity, new();

  /// <summary>
  /// Loads all children for an entity
  /// </summary>
  Task<Paged<T>> LoadChildren<T>(string parentId, int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default) where T : ZeroIdEntity, ISupportsTrees, new();

  /// <summary>
  /// Get descendants by query (by using the specified index)
  /// </summary>
  Task<Paged<T>> LoadChildren<T, TIndex>(string parentId, int pageNumber, int pageSize, Func<IRavenQueryable<T>, IQueryable<T>> querySelector = default) where T : ZeroIdEntity, ISupportsTrees, new() where TIndex : AbstractCommonApiForIndexes, new();

  /// <summary>
  /// Get tree hierarchy for an entity
  /// </summary>
  Task<T[]> GetHierarchy<T, TIndex>(string id) where T : ZeroIdEntity, ISupportsTrees, new() where TIndex : ZeroTreeHierarchyIndex<T>, new();

  /// <summary>
  /// Move an entity to a new parent
  /// </summary>
  Task<Result<T>> Move<T>(string id, string newParentId, Func<T, string, Task<bool>> isParentAllowed = null) where T : ZeroIdEntity, ISupportsTrees, new();

  /// <summary>
  /// Copies an entity to a new location
  /// </summary>
  Task<Result<T>> Copy<T>(string id, string newParentId, Func<T, string, Task<bool>> isParentAllowed = null) where T : ZeroIdEntity, ISupportsTrees, new();

  /// <summary>
  /// Copies an entity with descendants to a new location
  /// </summary>
  Task<Result<T>> CopyWithDescendants<T>(string id, string newParentId, Func<T, string, Task<bool>> isParentAllowed = null) where T : ZeroIdEntity, ISupportsTrees, new();

  /// <summary>
  /// Deletes an entity with all descendents
  /// </summary>
  Task<Result<string[]>> DeleteWithDescendants<T>(T model) where T : ZeroIdEntity, ISupportsTrees, new();
}


public interface ISharedStoreOperations : IStoreOperations { }
public interface IStoreOperationsWithInactive : IStoreOperations { }
public interface ISharedStoreOperationsWithInactive : IStoreOperations { }