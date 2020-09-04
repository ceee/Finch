using System;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using Rc = Raven.Client;

namespace zero.Core.Api
{
  public class PreviewApi : AppAwareBackofficeApi, IPreviewApi
  {
    IPreview Blueprint;

    public PreviewApi(IBackofficeStore store, IPreview blueprint) : base(store)
    {
      Blueprint = blueprint;
    }


    /// <inheritdoc />
    public async Task<EntityResult<IPreview>> Add<TEntity>(TEntity model) where TEntity : IZeroEntity
    {
      return await Update(null, model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IPreview>> Update<TEntity>(string id, TEntity model) where TEntity : IZeroEntity
    {
      IPreview entity = id == null ? await GetById<IPreview>(id) : Blueprint.Clone();
      entity.Content = model;
      entity.OriginalId = model.Id;
      entity.Name = model.Name;

      return await SaveModel(entity, meta: x =>
      {
        x[Rc.Constants.Documents.Metadata.Expires] = GetExpiry(model);
      });
    }


    /// <inheritdoc />
    public async Task<IPreview> GetById(string id)
    {
      return await GetById<IPreview>(id);
    }


    /// <summary>
    /// Get preview expiration for a document
    /// </summary>
    DateTime GetExpiry(IZeroEntity model)
    {
      return DateTime.Now.AddHours(1);
    }
  }

  public interface IPreviewApi
  {
    /// <summary>
    /// Adds an entity to the preview collection. This will generate a preview with an id which can be used for the preview view.
    /// </summary>
    Task<EntityResult<IPreview>> Add<TEntity>(TEntity model) where TEntity : IZeroEntity;

    /// <summary>
    /// Updates an entity in the preview collection. This will generate a preview with an id which can be used for the preview view.
    /// </summary>
    Task<EntityResult<IPreview>> Update<TEntity>(string id, TEntity model) where TEntity : IZeroEntity;

    /// <summary>
    /// Get preview entity by Id
    /// </summary>
    Task<IPreview> GetById(string id);
  }
}
