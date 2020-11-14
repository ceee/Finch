using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Utils;

namespace zero.Core.Api
{
  public class RecycleBinApi : BackofficeApi, IRecycleBinApi
  {
    IRecycledEntity Blueprint;

    public RecycleBinApi(IBackofficeStore store, IRecycledEntity blueprint) : base(store)
    {
      Blueprint = blueprint;
    }


    /// <inheritdoc />
    public async Task<EntityResult<IRecycledEntity>> Add<TEntity>(TEntity model, string group = null, string operationId = null) where TEntity : IZeroEntity
    {
      IRecycledEntity entity = Blueprint.Clone();
      entity.Group = group;
      entity.Content = model;
      entity.OriginalId = model.Id;
      entity.OperationId = operationId;
      entity.Name = model.Name;

      return await SaveModel(entity);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IEnumerable<IRecycledEntity>>> Add<TEntity>(IEnumerable<TEntity> models, string group = null) where TEntity : IZeroEntity
    {
      IList<IRecycledEntity> results = new List<IRecycledEntity>();
      string operationId = IdGenerator.Create();

      foreach (TEntity model in models)
      {
        EntityResult<IRecycledEntity> result = await Add(model, group, operationId);

        if (!result.IsSuccess)
        {
          return EntityResult<IEnumerable<IRecycledEntity>>.Fail(result.Errors);
        }

        results.Add(result.Model);
      }

      return EntityResult<IEnumerable<IRecycledEntity>>.Success(results);
    }


    /// <inheritdoc />
    public async Task<IRecycledEntity> GetById(string id)
    {
      return await GetById<IRecycledEntity>(id);
    }


    /// <inheritdoc />
    public async Task<ListResult<IRecycledEntity>> GetByQuery(RecycleBinListQuery query)
    {
      query.SearchSelector = x => x.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<IRecycledEntity>()
          .WhereIf(x => x.Group == query.Group, !query.Group.IsNullOrWhiteSpace())
          .WhereIf(x => x.OperationId == query.OperationId, !query.OperationId.IsNullOrWhiteSpace())
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<IList<IRecycledEntity>> GetByOperation(string operationId)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<IRecycledEntity>()
          .Where(x => x.OperationId == operationId)
          .ToListAsync();
      }
    }


    /// <summary>
    /// Get affected entity count from a specific operation
    /// </summary>
    public async Task<int> GetCountByOperation(string operationId)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<IRecycledEntity>()
          .Where(x => x.OperationId == operationId)
          .CountAsync();
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<IRecycledEntity>> Delete(string id)
    {
      return await DeleteById<IRecycledEntity>(id);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IRecycledEntity>> DeleteAll()
    {
      // TODO make Purge operations app-aware
      return await Purge<IRecycledEntity>();
    }


    /// <inheritdoc />
    public async Task<EntityResult<IRecycledEntity>> DeleteByOperation(string operationId)
    {
      return await Purge<IRecycledEntity>("where c.OperationId = $id", new Raven.Client.Parameters() { { "id", operationId } });
    }


    /// <inheritdoc />
    public async Task<EntityResult<IRecycledEntity>> DeleteByGroup(string group)
    {
      return await Purge<IRecycledEntity>("where c.Group = $group", new Raven.Client.Parameters() { { "group", group } });
    }
  }

  public interface IRecycleBinApi
  {
    /// <summary>
    /// Adds an entity to the recycle bin. This operation will not remove this entity from it's own collection!
    /// </summary>
    Task<EntityResult<IRecycledEntity>> Add<TEntity>(TEntity model, string group = null, string operationId = null) where TEntity : IZeroEntity;

    /// <summary>
    /// Adds the specified entities to the recycle bin. This operation will not remove entities from their own collection!
    /// </summary>
    Task<EntityResult<IEnumerable<IRecycledEntity>>> Add<TEntity>(IEnumerable<TEntity> models, string group = null) where TEntity : IZeroEntity;

    /// <summary>
    /// Get recycled entity by Id
    /// </summary>
    Task<IRecycledEntity> GetById(string id);

    /// <summary>
    /// Get all recycled items
    /// </summary>
    Task<ListResult<IRecycledEntity>> GetByQuery(RecycleBinListQuery query);

    /// <summary>
    /// Get affected entities from a specific operation
    /// </summary>
    Task<IList<IRecycledEntity>> GetByOperation(string operationId);

    /// <summary>
    /// Get affected entity count from a specific operation
    /// </summary>
    Task<int> GetCountByOperation(string operationId);

    // <summary>
    /// Deletes a recycled entity by Id
    /// </summary>
    Task<EntityResult<IRecycledEntity>> Delete(string id);

    /// <summary>
    /// Purges the recycle bin
    /// </summary>
    Task<EntityResult<IRecycledEntity>> DeleteAll();

    /// <summary>
    /// Deletes all recycled items from a specific operation
    /// </summary>
    Task<EntityResult<IRecycledEntity>> DeleteByOperation(string operationId);

    /// <summary>
    /// Deletes all recycled items from a specific group
    /// </summary>
    Task<EntityResult<IRecycledEntity>> DeleteByGroup(string group);
  }
}
