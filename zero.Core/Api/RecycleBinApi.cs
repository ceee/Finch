using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Utils;

namespace zero.Core.Api
{
  public class RecycleBinApi<T> : AppAwareBackofficeApi, IRecycleBinApi<T> where T : IRecycledEntity, new()
  {
    public RecycleBinApi(IBackofficeStore store) : base(store)
    {
      
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Add<TEntity>(TEntity model, string group = null, string operationId = null) where TEntity : IZeroEntity
    {
      T entity = new T()
      {
        Group = group,
        Content = null,
        OriginalId = model.Id,
        OperationId = operationId,
        Name = model.Name
      };

      return await SaveModel(entity);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IEnumerable<T>>> Add<TEntity>(IEnumerable<TEntity> models, string group = null) where TEntity : IZeroEntity
    {
      IList<T> results = new List<T>();
      string operationId = IdGenerator.Create();

      foreach (TEntity model in models)
      {
        EntityResult<T> result = await Add(model, group, operationId);

        if (!result.IsSuccess)
        {
          return EntityResult<IEnumerable<T>>.Fail(result.Errors);
        }

        results.Add(result.Model);
      }

      return EntityResult<IEnumerable<T>>.Success(results);
    }


    /// <inheritdoc />
    public async Task<ListResult<T>> GetByQuery(RecycleBinListQuery<T> query)
    {
      query.SearchSelector = x => x.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>()
          .Scope(Scope)
          .WhereIf(x => x.Group == query.Group, !query.Group.IsNullOrWhiteSpace())
          .WhereIf(x => x.OperationId == query.OperationId, !query.OperationId.IsNullOrWhiteSpace())
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task Delete(string id)
    {
      await DeleteById<T>(id);
    }


    /// <inheritdoc />
    public async Task DeleteAll()
    {
      // TODO make Purge operations app-aware
      await Purge<T>();
    }


    /// <inheritdoc />
    public async Task DeleteByOperation(string operationId)
    {
      await Purge<T>("where OperationId = $id", new Raven.Client.Parameters() { { "id", operationId } });
    }


    /// <inheritdoc />
    public async Task DeleteByGroup(string group)
    {
      await Purge<T>("where Group = $group", new Raven.Client.Parameters() { { "group", group } });
    }
  }


  public interface IRecycleBinApi<T> where T : IRecycledEntity
  {
    /// <summary>
    /// Adds an entity to the recycle bin. This operation will not remove this entity from it's own collection!
    /// </summary>
    Task<EntityResult<T>> Add<TEntity>(TEntity model, string group = null, string operationId = null) where TEntity : IZeroEntity;

    /// <summary>
    /// Adds the specified entities to the recycle bin. This operation will not remove entities from their own collection!
    /// </summary>
    Task<EntityResult<IEnumerable<T>>> Add<TEntity>(IEnumerable<TEntity> models, string group = null) where TEntity : IZeroEntity;

    /// <summary>
    /// Get all recycled items
    /// </summary>
    Task<ListResult<T>> GetByQuery(RecycleBinListQuery<T> query);

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
