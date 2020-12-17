using FluentValidation;
using FluentValidation.Results;
using Raven.Client;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Attributes;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Utils;

namespace zero.Core.Collections
{
  public abstract class CollectionBase<T> : ICollectionBase<T>, IDisposable where T : IZeroEntity
  {
    private IAsyncDocumentSession _session;
    private string _database;

    protected ICollectionInterceptorHandler InterceptorHandler { get; private set; }

    protected virtual Action<T> PreSave { get; set; }


    public CollectionBase(IZeroContext context, ICollectionInterceptorHandler interceptorHandler = null, IValidator<T> validator = null)
    {
      Context = context;
      Store = context.Store;
      InterceptorHandler = interceptorHandler;
      Validator = validator;
      Database = Store.ResolvedDatabase;
    }


    /// <summary>
    /// Zero context
    /// </summary>
    protected readonly IZeroContext Context;

    /// <summary>
    /// Document store
    /// </summary>
    protected readonly IZeroStore Store;

    /// <summary>
    /// The validator
    /// </summary>
    protected readonly IValidator<T> Validator;

    /// <summary>
    /// Create an an async document session
    /// </summary>
    protected IAsyncDocumentSession Session
    {
      get
      {
        if (_session != null)
        {
          return _session;
        }
        _session = Store.OpenAsyncSession(Database ?? Store.ResolvedDatabase);
        _session.Advanced.WaitForIndexesAfterSaveChanges(throwOnTimeout: false);
        return _session;
      }
    }

    /// <inheritdoc />
    public string Database
    {
      get => _database;
      set
      {
        if (value != _database)
        {
          _session?.Dispose();
          _session = null;
          _database = value;
        }
      }
    }

    /// <inheritdoc />
    public Guid Guid { get; private set; } = Guid.NewGuid();

    /// <inheritdoc />
    public IRavenQueryable<T> Query => Session.Query<T>();


    /// <inheritdoc />
    public virtual void ApplyScope(string scope)
    {
      Database = scope is "shared" or "core" ? Context.Options.Raven.Database : Store.ResolvedDatabase;
    }


    /// <inheritdoc />
    public virtual async Task<T> GetById(string id)
    {
      if (id.IsNullOrWhiteSpace())
      {
        return default;
      }

      return await Session.LoadAsync<T>(id);
    }


    /// <inheritdoc />
    public virtual async Task<Dictionary<string, T>> GetByIds(params string[] ids)
    {
      Dictionary<string, T> models = await Session.LoadAsync<T>(ids);
      Dictionary<string, T> result = new Dictionary<string, T>();

      foreach (string id in ids)
      {
        models.TryGetValue(id, out T model);
        result.Add(id, model);
      }

      return result;
    }


    /// <inheritdoc />
    public virtual async Task<ListResult<T>> GetByQuery(ListQuery<T> query)
    {
      return await Session.Query<T>().OrderByDescending(x => x.CreatedDate).ToQueriedListAsync(query);
    }


    /// <inheritdoc />
    public virtual async Task<List<T>> GetAll()
    {
      List<T> items = new();

      await foreach (T item in Stream())
      {
        items.Add(item);
      }

      return items;
    }


    /// <inheritdoc />
    public virtual IAsyncEnumerable<T> Stream() => Stream(null);


    /// <inheritdoc />
    public virtual async IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IRavenQueryable<T>> expression)
    {
      IRavenQueryable<T> query = Session.Query<T>();

      if (expression != null)
      {
        query = expression(query);
      }

      var stream = await Session.Advanced.StreamAsync(query);

      while (await stream.MoveNextAsync())
      {
        yield return stream.Current.Document;
      }
    }


    /// <inheritdoc />
    public virtual async Task<EntityResult<T>> Save(T model)
    {
      if (model == null)
      {
        return EntityResult<T>.Fail("@errors.onsave.empty");
      }

      PreSave?.Invoke(model);

      bool isCreate = false;

      // find all Raven Ids
      List<ObjectTraverser.Result<GenerateIdAttribute>> ravenIds = ObjectTraverser.FindAttribute<GenerateIdAttribute>(model);

      // set unset Raven Ids
      foreach (ObjectTraverser.Result<GenerateIdAttribute> item in ravenIds)
      {
        string id = item.Property.GetValue(item.Parent, null) as string;
        if (String.IsNullOrWhiteSpace(id))
        {
          item.Property.SetValue(item.Parent, item.Item.Length.HasValue ? IdGenerator.Create(item.Item.Length.Value) : IdGenerator.Create());
        }
      }

      // get current user
      string userId = Context.BackofficeUser.FindFirstValue(Constants.Auth.Claims.UserId);

      // set default properties
      if (model.Id.IsNullOrEmpty())
      {
        isCreate = true;

        model.CreatedDate = DateTimeOffset.Now;
        model.CreatedById = userId;

        if (model is ILanguageAwareEntity)
        {
          (model as ILanguageAwareEntity).LanguageId = "languages.1-A"; // TODO correct language id
        }
      }

      // update name alias and last modified
      model.Alias = Safenames.Alias(model.Name);
      model.LastModifiedById = userId;
      model.LastModifiedDate = DateTimeOffset.Now;
      model.CreatedById ??= userId;
      model.Hash ??= IdGenerator.Create();

      // create interceptor parameters
      CollectionInterceptor.Parameters<T> parameters = default;
      if (isCreate)
      {
        parameters = Parameters<CollectionInterceptor.CreateParameters<T>>(args => args.Model = model);
      }
      else
      {
        parameters = Parameters<CollectionInterceptor.UpdateParameters<T>>(args =>
        {
          args.Id = model.Id;
          args.Model = model;
        });
      }

      // run interceptors
      if (isCreate)
      {
        return await Create(model);
      }

      return await Update(model);
    }


    /// <inheritdoc />
    async Task<EntityResult<T>> Create(T model)
    {
      // run interceptors
      var parameters = Parameters<CollectionInterceptor.CreateParameters<T>>(args => args.Model = model);
      EntityResult<T> preResult = await InterceptorHandler?.Handle(x => x.Creating(parameters));

      if (preResult != null)
      {
        return preResult;
      }

      // run validator
      if (Validator != null)
      {
        ValidationResult validation = await Validator.ValidateAsync(model);

        if (!validation.IsValid)
        {
          return EntityResult<T>.Fail(validation);
        }
      }

      await Session.StoreAsync(model);

      // run interceptors
      await InterceptorHandler?.Handle<T>(x => x.Created(parameters));

      await Session.SaveChangesAsync();

      return EntityResult<T>.Success(model);
    }


    /// <inheritdoc />
    async Task<EntityResult<T>> Update(T model)
    {
      // run interceptors
      var parameters = Parameters<CollectionInterceptor.UpdateParameters<T>>(args =>
      {
        args.Model = model;
        args.Id = model.Id;
      });
      EntityResult<T> preResult = await InterceptorHandler?.Handle(x => x.Updating(parameters));

      if (preResult != null)
      {
        return preResult;
      }

      // run validator
      if (Validator != null)
      {
        ValidationResult validation = await Validator.ValidateAsync(model);

        if (!validation.IsValid)
        {
          return EntityResult<T>.Fail(validation);
        }
      }

      await Session.StoreAsync(model);

      // run interceptors
      await InterceptorHandler?.Handle<T>(x => x.Updated(parameters));

      await Session.SaveChangesAsync();

      return EntityResult<T>.Success(model);
    }


    /// <inheritdoc />
    public virtual async Task<EntityResult<T>> Delete(T model) => await DeleteById(model?.Id);


    /// <inheritdoc />
    public virtual async Task<EntityResult<T>> DeleteById(string id)
    {
      T entity = await Session.LoadAsync<T>(id);

      if (entity == null)
      {
        return EntityResult<T>.Fail("@errors.ondelete.idnotfound");
      }

      var parameters = Parameters<CollectionInterceptor.DeleteParameters<T>>(args =>
      {
        args.Model = entity;
        args.Id = entity.Id;
      });
      EntityResult<T> preResult = await InterceptorHandler?.Handle(x => x.Deleting(parameters));

      if (preResult != null)
      {
        return preResult;
      }

      Session.Delete(entity);

      await InterceptorHandler?.Handle<T>(x => x.Deleted(parameters));

      await Session.SaveChangesAsync();

      return EntityResult<T>.Success();
    }


    /// <inheritdoc />
    public virtual async Task<int> Delete(params T[] models) => await DeleteByIds(models.Select(x => x.Id).ToArray());


    /// <inheritdoc />
    public virtual async Task<int> DeleteByIds(params string[] ids)
    {
      int successCount = 0;

      foreach (string id in ids)
      {
        EntityResult<T> result = await DeleteById(id);
        successCount += result.IsSuccess ? 1 : 0;
      }

      return successCount;
    }


    /// <inheritdoc />
    public virtual async Task<EntityResult<T>> Purge(string querySuffix = null, Parameters parameters = null)
    {
      var interceptorParameters = Parameters<CollectionInterceptor.PurgeParameters<T>>();
      EntityResult<T> preResult = await InterceptorHandler?.Handle(x => x.Purging(interceptorParameters));

      if (preResult != null)
      {
        return preResult;
      }

      await Store.PurgeAsync<T>(Database, querySuffix, parameters);

      await InterceptorHandler?.Handle<T>(x => x.Purged(interceptorParameters));

      return EntityResult<T>.Success();
    }


    /// <inheritdoc />
    public void Dispose()
    {
      Session?.Dispose();
    }


    /// <summary>
    /// Get interceptor parameters
    /// </summary>
    public TParams Parameters<TParams>(Action<TParams> configure = null) where TParams : CollectionInterceptor.Parameters<T>, new()
    {
      TParams parameters = new TParams()
      {
        Context = Context,
        Store = Store,
        Validator = Validator,
        Session = Session
      };
      configure?.Invoke(parameters);
      return parameters;
    }
  }


  public interface ICollectionBase<T> : IDisposable where T : IZeroEntity
  {
    /// <summary>
    /// Guid for this instance
    /// </summary>
    Guid Guid { get; }

    /// <summary>
    /// The database to operate on.
    /// Is null by default, which uses the database from the resolved application.
    /// </summary>
    string Database { get; set; }

    /// <summary>
    /// Returns a new document queryable
    /// </summary>
    IRavenQueryable<T> Query { get; }

    /// <summary>
    /// Applies the scope to the service instance
    /// </summary>
    void ApplyScope(string scope);

    /// <summary>
    /// Get an entity by Id
    /// </summary>
    Task<T> GetById(string id);

    /// <summary>
    /// Get entities by ids
    /// </summary>
    Task<Dictionary<string, T>> GetByIds(params string[] ids);

    /// <summary>
    /// Get entities by query
    /// </summary>
    Task<ListResult<T>> GetByQuery(ListQuery<T> query);

    /// <summary>
    /// Get all entities from this collection. 
    /// Warning: Don't use this method for large collections. Stream the results instead.
    /// </summary>
    Task<List<T>> GetAll();

    /// <summary>
    /// Stream the collection
    /// </summary>
    IAsyncEnumerable<T> Stream();

    /// <summary>
    /// Stream the collection
    /// </summary>
    IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IRavenQueryable<T>> expression);

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
    Task<int> Delete(params T[] models);

    /// <summary>
    /// Deletes an entity by Id
    /// </summary>
    Task<EntityResult<T>> DeleteById(string id);

    /// <summary>
    /// Deletes entities by Id
    /// </summary>
    Task<int> DeleteByIds(params string[] ids);

    /// <summary>
    /// Delete a whole collection (with an optional query suffix, i.e. a where statement)
    /// </summary>
    Task<EntityResult<T>> Purge(string querySuffix = null, Parameters parameters = null);
  }
}
