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
  public abstract class CollectionBase<T> : ICollectionBase<T>, IDisposable where T : ZeroEntity
  {
    private IAsyncDocumentSession _session;
    private IRevisionsApi _revisions;
    private string _database;

    protected ICollectionInterceptorHandler InterceptorHandler { get; private set; }

    protected virtual Action<T> PreSave { get; set; }

    protected bool OnlyActive { get; set; } = false; // TODO do we really need this?


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
    /// Manage revisions
    /// </summary>
    protected IRevisionsApi Revisions
    {
      get
      {
        if (_revisions != null)
        {
          return _revisions;
        }

        _revisions = new RevisionsApi(Session);
        return _revisions;
      }
    }

    /// <summary>
    /// Create an an async document session
    /// </summary>
    public IAsyncDocumentSession Session
    {
      get
      {
        if (_session != null)
        {
          return _session;
        }
        _session = Context.GetCurrentScopeAsyncSession(Database ?? Store.ResolvedDatabase);
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
    public virtual void WithInactive(bool includeInactive = true)
    {
      OnlyActive = !includeInactive;
    }


    /// <inheritdoc />
    public virtual async Task<T> GetById(string id, string changeVector = null)
    {
      if (id.IsNullOrWhiteSpace())
      {
        return default;
      }
      if (!changeVector.IsNullOrEmpty())
      {
        return WhenActive(await GetRevision(changeVector));
      }

      return WhenActive(await Session.LoadAsync<T>(id));
    }


    /// <inheritdoc />
    public virtual async Task<Dictionary<string, T>> GetByIds(params string[] ids)
    {
      ids = ids.Distinct().ToArray();

      Dictionary<string, T> models = await Session.LoadAsync<T>(ids);
      Dictionary<string, T> result = new Dictionary<string, T>();

      foreach (string id in ids)
      {
        models.TryGetValue(id, out T model);
        result.Add(id, WhenActive(model));
      }

      return result;
    }


    /// <inheritdoc />
    public virtual async Task<ListResult<T>> GetByQuery(ListQuery<T> query)
    {
      if (query.SearchSelector == null && !query.SearchSelectors.Any())
      {
        query.SearchSelector = x => x.Name;
      }
      return await Session.Query<T>().WhereIf(x => x.IsActive, OnlyActive).ToQueriedListAsync(query);
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
    public virtual async Task<ListResult<Revision>> GetRevisions(string id, int page = 1, int pageSize = 10)
    {
      if (id.IsNullOrWhiteSpace())
      {
        return default;
      }

      return await Revisions.GetPaged<T>(id, page, pageSize);
    }


    /// <inheritdoc />
    public virtual async Task<T> GetRevision(string changeVector)
    {
      return await Session.Advanced.Revisions.GetAsync<T>(changeVector);
    }


    /// <inheritdoc />
    public virtual IAsyncEnumerable<T> Stream() => Stream(null);


    /// <inheritdoc />
    public virtual async IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IRavenQueryable<T>> expression)
    {
      IRavenQueryable<T> query = Session.Query<T>().WhereIf(x => x.IsActive, OnlyActive);

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

      // set IDs
      model.AutoSetIds();

      // get current user
      string userId = Context.BackofficeUser.FindFirstValue(Constants.Auth.Claims.UserId);

      // set default properties
      if (model.Id.IsNullOrEmpty())
      {
        isCreate = true;

        model.CreatedDate = DateTimeOffset.Now;
        model.CreatedById = userId;
        model.LanguageId ??= "languages.1-A"; // TODO correct language id
      }

      // update name alias and last modified
      model.Alias = Safenames.Alias(model.Name);
      model.LastModifiedById = userId;
      model.LastModifiedDate = DateTimeOffset.Now;
      model.CreatedById ??= userId;
      model.Hash ??= IdGenerator.Classic();

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
      // run validator
      if (Validator != null)
      {
        ValidationResult validation = await Validator.ValidateAsync(model);

        if (!validation.IsValid)
        {
          return EntityResult<T>.Fail(validation);
        }
      }

      // run interceptor
      var instruction = CreateInstruction<CollectionInterceptor<T>.CreateParameters>("create", args => args.Model = model);
      await instruction.HandleBefore(x => x.Creating(instruction.Parameters));

      if (instruction.Return)
      {
        return instruction.EntityResult;
      }

      // run generic interceptor
      var instruction2 = CreateInstruction<CollectionInterceptor<T>.SaveParameters>("save", args =>
      {
        args.Model = model;
        args.Id = model.Id;
      });
      await instruction2.HandleBefore(x => x.Saving(instruction2.Parameters));

      if (instruction2.Return)
      {
        return instruction2.EntityResult;
      }

      await Session.StoreAsync(model);

      await instruction.HandleAfter(x => x.Created(instruction.Parameters));
      await instruction2.HandleAfter(x => x.Saved(instruction2.Parameters));

      await Session.SaveChangesAsync();

      return EntityResult<T>.Success(model);
    }


    /// <inheritdoc />
    async Task<EntityResult<T>> Update(T model)
    {
      // run validator
      if (Validator != null)
      {
        ValidationResult validation = await Validator.ValidateAsync(model);

        if (!validation.IsValid)
        {
          return EntityResult<T>.Fail(validation);
        }
      }

      // run interceptor
      var instruction = CreateInstruction<CollectionInterceptor<T>.UpdateParameters>("update", args =>
      {
        args.Model = model;
        args.Id = model.Id;
      });
      await instruction.HandleBefore(x => x.Updating(instruction.Parameters));

      if (instruction.Return)
      {
        return instruction.EntityResult;
      }

      // run generic interceptor
      var instruction2 = CreateInstruction<CollectionInterceptor<T>.SaveParameters>("save", args =>
      {
        args.Model = model;
        args.Id = model.Id;
      });
      await instruction2.HandleBefore(x => x.Saving(instruction2.Parameters));

      if (instruction2.Return)
      {
        return instruction2.EntityResult;
      }

      await Session.StoreAsync(model);

      await instruction.HandleAfter(x => x.Updated(instruction.Parameters));
      await instruction2.HandleAfter(x => x.Saved(instruction2.Parameters));

      await Session.SaveChangesAsync();

      return EntityResult<T>.Success(model);
    }


    /// <inheritdoc />
    public virtual async Task<EntityResult<T>> Delete(T model) => await DeleteById(model?.Id);


    /// <inheritdoc />
    public virtual async Task<EntityResult<T>> DeleteById(string id)
    {
      if (String.IsNullOrEmpty(id))
      {
        return EntityResult<T>.Fail("@errors.ondelete.idnotfound");
      }

      T entity = await Session.LoadAsync<T>(id);

      if (entity == null)
      {
        return EntityResult<T>.Fail("@errors.ondelete.idnotfound");
      }

      var instruction = CreateInstruction<CollectionInterceptor<T>.DeleteParameters>("delete", args =>
      {
        args.Model = entity;
        args.Id = entity.Id;
      });
      await instruction.HandleBefore(x => x.Deleting(instruction.Parameters));

      if (instruction.Return)
      {
        return instruction.EntityResult;
      }

      Session.Delete(entity);

      await instruction.HandleAfter(x => x.Deleted(instruction.Parameters));

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
      var instruction = CreateInstruction<CollectionInterceptor<T>.PurgeParameters>("purge");
      await instruction.HandleBefore(x => x.Purging(instruction.Parameters));

      if (instruction.Return)
      {
        return instruction.EntityResult;
      }

      await Store.PurgeAsync<T>(Database, querySuffix, parameters);

      await instruction.HandleAfter(x => x.Purged(instruction.Parameters));

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
    protected InterceptorInstruction<T, TParams> CreateInstruction<TParams>(string operationName, Action<TParams> configure = null) where TParams : CollectionInterceptor<T>.Parameters, new()
    {
      TParams parameters = new TParams()
      {
        Context = Context,
        Store = Store,
        Validator = Validator,
        Session = Session
      };
      configure?.Invoke(parameters);

      return InterceptorHandler?.Create<T, TParams>(operationName, parameters) ?? new();
    }


    /// <summary>
    /// Do only return the model when it is set to active or inactive entities are included with IncludeInactive()
    /// </summary>
    protected TRes WhenActive<TRes>(TRes model) where TRes : T
    {
      return model != null && (!OnlyActive || model.IsActive) ? model : default;
    }
  }


  public interface ICollectionBase<T> : IDisposable where T : ZeroEntity
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
    /// Create an async document session
    /// </summary>
    IAsyncDocumentSession Session { get; }

    /// <summary>
    /// Applies the scope to the service instance
    /// </summary>
    void ApplyScope(string scope);

    /// <summary>
    /// Include entities with IsActive=false for GET queries
    /// </summary>
    void WithInactive(bool include = true);

    /// <summary>
    /// Get an entity by Id
    /// </summary>
    Task<T> GetById(string id, string changeVector = null);

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
    /// Get page revisions for the specified entity
    /// </summary>
    Task<ListResult<Revision>> GetRevisions(string id, int page = 1, int pageSize = 10);

    /// <summary>
    /// Get a revision by change vector
    /// </summary>
    Task<T> GetRevision(string changeVector);

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
