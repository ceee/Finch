using FluentValidation.Results;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using System.Security.Claims;

namespace zero.Collections;

public abstract partial class CollectionOperations : ICollectionOperations
{
  /// <inheritdoc />
  public IZeroDocumentSession Session => Context.Store.Session();

  protected record EntityCollectionOptions(bool IncludeInactive);

  protected IZeroContext Context { get; private set; }

  protected EntityCollectionOptions Options { get; set; }

  protected IInterceptors Interceptors { get; private set; }


  public CollectionOperations(ICollectionContext collectionContext)
  {
    Context = collectionContext.Context;
    Interceptors = collectionContext.Interceptors;
    Options = new(true);
  }


  /// <summary>
  /// Get new instance of an entity
  /// </summary>
  public virtual Task<T> Empty<T>() where T : ZeroIdEntity, new()
  {
    return Task.FromResult(new T());
  }


  /// <inheritdoc />
  public async Task<string> GenerateId<T>(T model) where T : ZeroIdEntity
  {
    IZeroDocumentSession session = Context.Store.Session();
    return await session.Advanced.DocumentStore.Conventions.GenerateDocumentIdAsync(session.Advanced.DocumentStore.Database, model);
  }


  /// <inheritdoc />
  public T AutoSetIds<T>(T model)
  {
    // find all Raven Ids
    List<ObjectTraverser.Result<GenerateIdAttribute>> ravenIds = ObjectTraverser.FindAttribute<GenerateIdAttribute>(model);

    // set unset Raven Ids
    foreach (ObjectTraverser.Result<GenerateIdAttribute> item in ravenIds)
    {
      string id = item.Property.GetValue(item.Parent, null) as string;
      if (id.IsNullOrWhiteSpace())
      {
        id = item.Item.Length.HasValue ? IdGenerator.Create(item.Item.Length.Value) : IdGenerator.Create();
        item.Property.SetValue(item.Parent, id);
      }
    }

    return model;
  }


  /// <inheritdoc />
  public T PrepareForSave<T>(T model) where T : ZeroIdEntity
  {
    // set IDs
    AutoSetIds(model);

    if (model is ZeroEntity)
    {
      ZeroEntity zeroModel = model as ZeroEntity;

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

    return model;
  }


  /// <summary>
  /// Do only return the model when it is set to active or inactive entities are included with IncludeInactive()
  /// </summary>
  protected virtual T WhenActive<T>(T model) where T : ZeroIdEntity, new()
  {
    return model != null && (Options.IncludeInactive || (model is ZeroEntity ? (model as ZeroEntity).IsActive : true)) ? model : default;
  }
}



public interface ICollectionOperations
{
  /// <summary>
  /// Get new instance of an entity
  /// </summary>
  Task<T> Empty<T>() where T : ZeroIdEntity, new();

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
  Task<ListResult<T>> Load<T>(ListQuery<T> query) where T : ZeroIdEntity, new();

  /// <summary>
  /// Get entities by query (by using the specified index)
  /// </summary>
  Task<ListResult<T>> Load<T, TIndex>(ListQuery<T> query) where T : ZeroIdEntity, new() where TIndex : AbstractCommonApiForIndexes, new();

  /// <summary>
  /// Get all entities from this collection. 
  /// Warning: Don't use this method for large collections. Stream the results instead.
  /// </summary>
  Task<List<T>> LoadAll<T>() where T : ZeroIdEntity, new();

  /// <summary>
  /// Stream the collection
  /// </summary>
  IAsyncEnumerable<T> Stream<T>() where T : ZeroIdEntity, new();

  /// <summary>
  /// Stream the collection
  /// </summary>
  IAsyncEnumerable<T> Stream<T>(Func<IRavenQueryable<T>, IRavenQueryable<T>> expression) where T : ZeroIdEntity, new();

  /// <summary>
  /// Updates or creates an entity with an optional validator
  /// </summary>
  Task<EntityResult<T>> Save<T>(T model, Func<T, Task<ValidationResult>> validate = null) where T : ZeroIdEntity, new();

  /// <summary>
  /// Deletes an entity
  /// </summary>
  Task<EntityResult<T>> Delete<T>(T model) where T : ZeroIdEntity, new();

  /// <summary>
  /// Deletes entities
  /// </summary>
  Task<int> Delete<T>(IEnumerable<T> models) where T : ZeroIdEntity, new();

  /// <summary>
  /// Deletes an entity by Id
  /// </summary>
  Task<EntityResult<T>> Delete<T>(string id) where T : ZeroIdEntity, new();

  /// <summary>
  /// Deletes entities by Id
  /// </summary>
  Task<int> Delete<T>(IEnumerable<string> ids) where T : ZeroIdEntity, new();
}