using System;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Extensions;
using Rc = Raven.Client;

namespace zero.Core.Api
{
  public class PreviewApi : BackofficeApi, IPreviewApi
  {
    Preview Blueprint;

    public PreviewApi(ICollectionContext store, Preview blueprint) : base(store)
    {
      Blueprint = blueprint;
    }


    /// <inheritdoc />
    public async Task<EntityResult<Preview>> Add<TEntity>(TEntity model) where TEntity : ZeroEntity
    {
      return await Update(null, model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Preview>> Update<TEntity>(string id, TEntity model) where TEntity : ZeroEntity
    {
      Preview entity = id == null ? await GetById<Preview>(id) : Blueprint.Clone();
      entity.Content = model;
      entity.OriginalId = model.Id;
      entity.Name = model.Name;

      return await SaveModel(entity, meta: x =>
      {
        x[Rc.Constants.Documents.Metadata.Expires] = GetExpiry(model);
      });
    }


    /// <inheritdoc />
    public async Task<Preview> GetById(string id)
    {
      return await GetById<Preview>(id);
    }


    /// <summary>
    /// Get preview expiration for a document
    /// </summary>
    DateTime GetExpiry(ZeroEntity model)
    {
      return DateTime.Now.AddHours(1);
    }
  }

  public interface IPreviewApi
  {
    /// <summary>
    /// Adds an entity to the preview collection. This will generate a preview with an id which can be used for the preview view.
    /// </summary>
    Task<EntityResult<Preview>> Add<TEntity>(TEntity model) where TEntity : ZeroEntity;

    /// <summary>
    /// Updates an entity in the preview collection. This will generate a preview with an id which can be used for the preview view.
    /// </summary>
    Task<EntityResult<Preview>> Update<TEntity>(string id, TEntity model) where TEntity : ZeroEntity;

    /// <summary>
    /// Get preview entity by Id
    /// </summary>
    Task<Preview> GetById(string id);
  }
}
