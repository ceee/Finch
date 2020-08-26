using Newtonsoft.Json;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Utils;

namespace zero.Core.Api
{
  public class RecycleBinApi : AppAwareBackofficeApi, IRecycleBinApi
  {
    Type Type;

    public RecycleBinApi(IBackofficeStore store, IRecycledEntity entity) : base(store)
    {
      Type = entity.GetType();
    }


    /// <inheritdoc />
    public async Task<EntityResult<IRecycledEntity>> Add<TEntity>(TEntity model, string group = null, string operationId = null) where TEntity : IZeroEntity
    {
      IRecycledEntity entity = Activator.CreateInstance(Type) as IRecycledEntity;
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
    public async Task<ListResult<IRecycledEntity>> GetByQuery(RecycleBinListQuery<IRecycledEntity> query)
    {
      query.SearchSelector = x => x.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<IRecycledEntity>()
          .Scope(Scope)
          .WhereIf(x => x.Group == query.Group, !query.Group.IsNullOrWhiteSpace())
          .WhereIf(x => x.OperationId == query.OperationId, !query.OperationId.IsNullOrWhiteSpace())
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task Delete(string id)
    {
      await DeleteById<IRecycledEntity>(id);
    }


    /// <inheritdoc />
    public async Task DeleteAll()
    {
      // TODO make Purge operations app-aware
      await Purge<IRecycledEntity>();
    }


    /// <inheritdoc />
    public async Task DeleteByOperation(string operationId)
    {
      await Purge<IRecycledEntity>("where c.OperationId = $id", new Raven.Client.Parameters() { { "id", operationId } });
    }


    /// <inheritdoc />
    public async Task DeleteByGroup(string group)
    {
      await Purge<IRecycledEntity>("where c.Group = $group", new Raven.Client.Parameters() { { "group", group } });
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
    /// Get all recycled items
    /// </summary>
    Task<ListResult<IRecycledEntity>> GetByQuery(RecycleBinListQuery<IRecycledEntity> query);

    // <summary>
    /// Deletes a country by Id
    /// </summary>
    Task Delete(string id);

    /// <summary>
    /// Purges the recycle bin
    /// </summary>
    Task DeleteAll();

    /// <summary>
    /// Deletes all recycled items from a specific operation
    /// </summary>
    Task DeleteByOperation(string operationId);

    /// <summary>
    /// Deletes all recycled items from a specific group
    /// </summary>
    Task DeleteByGroup(string group);
  }
}
