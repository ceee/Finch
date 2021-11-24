//using Raven.Client.Documents;
//using Raven.Client.Documents.Linq;
//using Raven.Client.Documents.Session;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using zero.Core.Collections;
//using zero.Core.Database.Indexes;
//using zero.Core.Entities;
//using zero.Core.Extensions;
//using zero.Core.Utils;

//namespace zero.Core.Api
//{
//  public class RecycleBinApi : BackofficeApi, IRecycleBinApi
//  {
//    RecycledEntity Blueprint;

//    public RecycleBinApi(IStoreContext store, RecycledEntity blueprint) : base(store)
//    {
//      Blueprint = blueprint;
//    }


//    /// <inheritdoc />
//    public async Task<EntityResult<RecycledEntity>> Add<TEntity>(TEntity model, string group = null, string operationId = null) where TEntity : ZeroEntity
//    {
//      RecycledEntity entity = Blueprint.Clone();
//      entity.Group = group;
//      entity.Content = model;
//      entity.OriginalId = model.Id;
//      entity.OperationId = operationId;
//      entity.Name = model.Name;

//      return await SaveModel(entity);
//    }


//    /// <inheritdoc />
//    public async Task<EntityResult<IEnumerable<RecycledEntity>>> Add<TEntity>(IEnumerable<TEntity> models, string group = null) where TEntity : ZeroEntity
//    {
//      IList<RecycledEntity> results = new List<RecycledEntity>();
//      string operationId = IdGenerator.Create();

//      foreach (TEntity model in models)
//      {
//        EntityResult<RecycledEntity> result = await Add(model, group, operationId);

//        if (!result.IsSuccess)
//        {
//          return EntityResult<IEnumerable<RecycledEntity>>.Fail(result.Errors);
//        }

//        results.Add(result.Model);
//      }

//      return EntityResult<IEnumerable<RecycledEntity>>.Success(results);
//    }


//    /// <inheritdoc />
//    public async Task<RecycledEntity> GetById(string id)
//    {
//      return await GetById<RecycledEntity>(id);
//    }


//    /// <inheritdoc />
//    public async Task<Paged<RecycledEntity>> GetByQuery(RecycleBinListQuery query)
//    {
//      query.SearchSelector = x => x.Name;

//      return await Session.Query<RecycledEntity, zero_RecycledEntities>()
//        .WhereIf(x => x.Group == query.Group, !query.Group.IsNullOrWhiteSpace())
//        .WhereIf(x => x.OperationId == query.OperationId, !query.OperationId.IsNullOrWhiteSpace())
//        .ToQueriedListAsync(query);
//    }


//    /// <inheritdoc />
//    public async Task<IList<RecycledEntity>> GetByOperation(string operationId)
//    {
//      return await Session.Query<RecycledEntity, zero_RecycledEntities>()
//        .Where(x => x.OperationId == operationId)
//        .ToListAsync();
//    }


//    /// <summary>
//    /// Get affected entity count from a specific operation
//    /// </summary>
//    public async Task<int> GetCountByOperation(string operationId)
//    {
//      return await Session.Query<RecycledEntity, zero_RecycledEntities>()
//        .Where(x => x.OperationId == operationId)
//        .CountAsync();
//    }


//    /// <inheritdoc />
//    public async Task<EntityResult<RecycledEntity>> Delete(string id)
//    {
//      return await DeleteById<RecycledEntity>(id);
//    }


//    /// <inheritdoc />
//    public async Task<EntityResult<RecycledEntity>> DeleteAll()
//    {
//      // TODO make Purge operations app-aware
//      return await Purge<RecycledEntity>();
//    }


//    /// <inheritdoc />
//    public async Task<EntityResult<RecycledEntity>> DeleteByOperation(string operationId)
//    {
//      return await Purge<RecycledEntity>("where c.OperationId = $id", new Raven.Client.Parameters() { { "id", operationId } });
//    }


//    /// <inheritdoc />
//    public async Task<EntityResult<RecycledEntity>> DeleteByGroup(string group)
//    {
//      return await Purge<RecycledEntity>("where c.Group = $group", new Raven.Client.Parameters() { { "group", group } });
//    }
//  }

//  public interface IRecycleBinApi
//  {
//    /// <summary>
//    /// Adds an entity to the recycle bin. This operation will not remove this entity from it's own collection!
//    /// </summary>
//    Task<EntityResult<RecycledEntity>> Add<TEntity>(TEntity model, string group = null, string operationId = null) where TEntity : ZeroEntity;

//    /// <summary>
//    /// Adds the specified entities to the recycle bin. This operation will not remove entities from their own collection!
//    /// </summary>
//    Task<EntityResult<IEnumerable<RecycledEntity>>> Add<TEntity>(IEnumerable<TEntity> models, string group = null) where TEntity : ZeroEntity;

//    /// <summary>
//    /// Get recycled entity by Id
//    /// </summary>
//    Task<RecycledEntity> GetById(string id);

//    /// <summary>
//    /// Get all recycled items
//    /// </summary>
//    Task<Paged<RecycledEntity>> GetByQuery(RecycleBinListQuery query);

//    /// <summary>
//    /// Get affected entities from a specific operation
//    /// </summary>
//    Task<IList<RecycledEntity>> GetByOperation(string operationId);

//    /// <summary>
//    /// Get affected entity count from a specific operation
//    /// </summary>
//    Task<int> GetCountByOperation(string operationId);

//    // <summary>
//    /// Deletes a recycled entity by Id
//    /// </summary>
//    Task<EntityResult<RecycledEntity>> Delete(string id);

//    /// <summary>
//    /// Purges the recycle bin
//    /// </summary>
//    Task<EntityResult<RecycledEntity>> DeleteAll();

//    /// <summary>
//    /// Deletes all recycled items from a specific operation
//    /// </summary>
//    Task<EntityResult<RecycledEntity>> DeleteByOperation(string operationId);

//    /// <summary>
//    /// Deletes all recycled items from a specific group
//    /// </summary>
//    Task<EntityResult<RecycledEntity>> DeleteByGroup(string group);
//  }
//}


/*
 * RESTORE FOR PAGES
 * 
 * 
 * 
  
/// <summary>
  /// Restores a page from the recycle bin
  /// </summary>
  Task<EntityResult<string[]>> Restore(string id, bool includeDescendants = false);


  /// <inheritdoc />
  public async Task<EntityResult<string[]>> Restore(string id, bool includeDescendants = false)
  {
    EntityResult<string[]> result = new EntityResult<string[]>();
    RecycledEntity recycledEntity = await RecycleBinApi.GetById(id);
    List<RecycledEntity> entities = new List<RecycledEntity>() { recycledEntity };

    if (recycledEntity == null)
    {
      return EntityResult<string[]>.Fail(); // TODO correct error message
    }

    // get descendants from the operation
    if (includeDescendants && !recycledEntity.OperationId.IsNullOrEmpty())
    {
      entities = (await RecycleBinApi.GetByOperation(recycledEntity.OperationId)).ToList();
    }

    // fill ids
    string[] ids = entities.Select(x => x.OriginalId).ToArray();

    // check if parents are available
    string[] parentIds = entities.Select(x => x.Content as Page).Where(x => x != null).Select(x => x.ParentId).ToArray();
    parentIds = (await Load(parentIds)).Where(x => x.Value != null).Select(x => x.Value.Id).ToArray();

    // validate and restore all items
    foreach (RecycledEntity entity in entities)
    {
      // check if it contains page data
      if (entity.Group != RECYCLE_BIN_GROUP || !(entity.Content is Page))
      {
        //result.AddError("Cannot parse recycled entity as an Page in group \"" + RECYCLE_BIN_GROUP + "\""); // TODO correct error message
        continue;
      }

      // get page
      Page page = entity.Content as Page;
      page.IsActive = false;

      // validate app and parent
      if (!page.ParentId.IsNullOrEmpty() && !ids.Contains(page.ParentId) && !parentIds.Contains(page.ParentId))
      {
        // TODO correct error message
        continue;
      }

      // restore to pages
      EntityResult<Page> saveResult = await Create(page);
    }

    // delete affected entities from recycle bin
    if (!recycledEntity.OperationId.IsNullOrEmpty())
    {
      await RecycleBinApi.DeleteByOperation(recycledEntity.OperationId);
    }

    // set result
    result.Model = ids;
    result.IsSuccess = true;

    return result;
  }

 */