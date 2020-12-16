//using FluentValidation;
//using FluentValidation.Results;
//using Raven.Client;
//using Raven.Client.Documents;
//using Raven.Client.Documents.Linq;
//using Raven.Client.Documents.Session;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using zero.Core.Api;
//using zero.Core.Attributes;
//using zero.Core.Database;
//using zero.Core.Entities;
//using zero.Core.Extensions;
//using zero.Core.Utils;

//namespace zero.Core.Collections
//{
//  public abstract class CollectionPresetBase<T> : ICollectionPresetBase<T>, IDisposable
//  {
//    private IAsyncDocumentSession _session;
//    private string _database;

//    protected ICollectionInterceptorHandler InterceptorHandler { get; private set; }

//    protected virtual Action<T> PreSave { get; set; }


//    public CollectionPresetBase(IZeroContext context, ICollectionInterceptorHandler interceptorHandler, IValidator<T> validator = null)
//    {
//      Context = context;
//      Store = context.Store;
//      InterceptorHandler = interceptorHandler;
//      Validator = validator;
//      Database = Store.ResolvedDatabase;
//    }


//    /// <summary>
//    /// Zero context
//    /// </summary>
//    protected readonly IZeroContext Context;

//    /// <summary>
//    /// Document store
//    /// </summary>
//    protected readonly IZeroStore Store;

//    /// <summary>
//    /// The validator
//    /// </summary>
//    protected readonly IValidator<T> Validator;

//    /// <summary>
//    /// Create an an async document session
//    /// </summary>
//    protected IAsyncDocumentSession Session
//    {
//      get
//      {
//        if (_session != null)
//        {
//          return _session;
//        }
//        _session = Store.OpenAsyncSession(Database ?? Store.ResolvedDatabase);
//        _session.Advanced.WaitForIndexesAfterSaveChanges(throwOnTimeout: false);
//        return _session;
//      }
//    }

//    /// <inheritdoc />
//    public string Database
//    {
//      get => _database;
//      set
//      {
//        if (value != _database)
//        {
//          _session?.Dispose();
//          _session = null;
//          _database = value;
//        }
//      }
//    }

//    /// <inheritdoc />
//    public Guid Guid { get; private set; } = Guid.NewGuid();


//    /// <inheritdoc />
//    public virtual void ApplyScope(string scope)
//    {
//      Database = scope is "shared" or "core" ? Context.Options.Raven.Database : Store.ResolvedDatabase;
//    }


//    /// <inheritdoc />
//    public virtual async Task<T> Get()
//    {
//      string alias = "// TODO";
//      IPresetOverride<T> preset = await Session.Query<IPresetOverride<T>>().FirstOrDefaultAsync(x => x.Alias == alias);


//    }


//    /// <inheritdoc />
//    public void Dispose()
//    {
//      Session?.Dispose();
//    }
//  }


//  public interface IICollectionPredefinedBase<T> : IDisposable where T : IZeroEntity
//  {
//    /// <summary>
//    /// Guid for this instance
//    /// </summary>
//    Guid Guid { get; }

//    /// <summary>
//    /// The database to operate on.
//    /// Is null by default, which uses the database from the resolved application.
//    /// </summary>
//    string Database { get; set; }

//    /// <summary>
//    /// Returns a new document queryable
//    /// </summary>
//    IRavenQueryable<T> Query { get; }

//    /// <summary>
//    /// Applies the scope to the service instance
//    /// </summary>
//    void ApplyScope(string scope);

//    /// <summary>
//    /// Get an entity by Id
//    /// </summary>
//    Task<T> GetById(string id);

//    /// <summary>
//    /// Get entities by ids
//    /// </summary>
//    Task<Dictionary<string, T>> GetByIds(params string[] ids);

//    /// <summary>
//    /// Get entities by query
//    /// </summary>
//    Task<ListResult<T>> GetByQuery(ListQuery<T> query);

//    /// <summary>
//    /// Get all entities from this collection. 
//    /// Warning: Don't use this method for large collections. Stream the results instead.
//    /// </summary>
//    Task<List<T>> GetAll();

//    /// <summary>
//    /// Stream the collection
//    /// </summary>
//    IAsyncEnumerable<T> Stream();

//    /// <summary>
//    /// Stream the collection
//    /// </summary>
//    IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IRavenQueryable<T>> expression);

//    /// <summary>
//    /// Updates or creates an entity with an optional validator
//    /// </summary>
//    Task<EntityResult<T>> Save(T model);

//    /// <summary>
//    /// Deletes an entity
//    /// </summary>
//    Task<EntityResult<T>> Delete(T model);

//    /// <summary>
//    /// Deletes entities
//    /// </summary>
//    Task<int> Delete(params T[] models);

//    /// <summary>
//    /// Deletes an entity by Id
//    /// </summary>
//    Task<EntityResult<T>> DeleteById(string id);

//    /// <summary>
//    /// Deletes entities by Id
//    /// </summary>
//    Task<int> DeleteByIds(params string[] ids);

//    /// <summary>
//    /// Delete a whole collection (with an optional query suffix, i.e. a where statement)
//    /// </summary>
//    Task<EntityResult<T>> Purge(string querySuffix = null, Parameters parameters = null);
//  }



//  public interface ICollectionPresetBase<T> : IDisposable
//  {
//    /// <summary>
//    /// Guid for this instance
//    /// </summary>
//    Guid Guid { get; }

//    /// <summary>
//    /// The database to operate on.
//    /// Is null by default, which uses the database from the resolved application.
//    /// </summary>
//    string Database { get; set; }

//    /// <summary>
//    /// Applies the scope to the service instance
//    /// </summary>
//    void ApplyScope(string scope);

//    /// <summary>
//    /// Get preset by Id
//    /// </summary>
//    Task<T> Get();
//  }
//}
