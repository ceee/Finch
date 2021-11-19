//namespace zero.Core;
//using FluentValidation.Results;
//using Raven.Client.Documents.Indexes;
//using Raven.Client.Documents.Linq;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using zero.Core.Database;
//using zero.Core.Entities;
//using zero.Core.Validation;

//public abstract partial class EntityCollectionProxy
//{
//  EntityCollection<T> Collection;

//  public async Task<ZeroIdEntity> Load(string id, string changeVector = null) => await Collection.Load(id, changeVector);
//}



//public interface IEntityCollection<T> where T : ZeroIdEntity
//{
//  /// <summary>
//  /// Get an entity by Id
//  /// </summary>
//  Task<T> Load(string id, string changeVector = null);

//  /// <summary>
//  /// Get entities by ids
//  /// </summary>
//  Task<Dictionary<string, T>> Load(params string[] ids);

//  /// <summary>
//  /// Get entities by query
//  /// </summary>
//  Task<ListResult<T>> Load(ListQuery<T> query);

//  /// <summary>
//  /// Get entities by query (by using the specified index)
//  /// </summary>
//  Task<ListResult<T>> Load<TIndex>(ListQuery<T> query) where TIndex : AbstractCommonApiForIndexes, new();

//  /// <summary>
//  /// Get all entities from this collection. 
//  /// Warning: Don't use this method for large collections. Stream the results instead.
//  /// </summary>
//  Task<List<T>> LoadAll();

//  /// <summary>
//  /// Stream the collection
//  /// </summary>
//  IAsyncEnumerable<T> Stream();

//  /// <summary>
//  /// Stream the collection
//  /// </summary>
//  IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IRavenQueryable<T>> expression);


//  /// <summary>
//  /// Validates an entity in this collection
//  /// </summary>
//  Task<ValidationResult> Validate(T model);
//}