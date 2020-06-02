using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class MediaApi : AppAwareBackofficeApi, IMediaApi
  {
    public MediaApi(IBackofficeStore store) : base(store) { }


    /// <inheritdoc />
    public async Task<Media> GetById(string id)
    {
      return await GetById<Media>(id);
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, Media>> GetById(IEnumerable<string> ids)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.LoadAsync<Media>(ids);
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<Media>> GetByQuery(MediaListQuery query)
    {
      query.SearchFor(entity => entity.Name);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<Media>()
          .Scope(Scope)
          .WhereIf(x => x.FolderId == query.FolderId, !query.FolderId.IsNullOrEmpty(), x => x.FolderId == null)
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<Media>> Save(Media model)
    {
      model.IsActive = true;
      return await SaveModel(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Media>> Delete(string id)
    {
      return await DeleteById<Media>(id);
    }


    /// <inheritdoc />
    public async Task Cleanup()
    {
      await Task.Delay(0);
    }
  }


  public interface IMediaApi : IAppAwareBackofficeApi
  {
    /// <summary>
    /// Get media by Id
    /// </summary>
    Task<Media> GetById(string id);

    /// <summary>
    /// Get media by ids
    /// </summary>
    Task<Dictionary<string, Media>> GetById(IEnumerable<string> ids);

    /// <summary>
    /// Get all available media items (with query)
    /// </summary>
    Task<ListResult<Media>> GetByQuery(MediaListQuery query);

    /// <summary>
    /// Creates or updates a application
    /// </summary>
    Task<EntityResult<Media>> Save(Media model);

    /// <summary>
    /// Deletes a application by Id
    /// </summary>
    Task<EntityResult<Media>> Delete(string id);

    /// <summary>
    /// Clean-up all media based on stored database information
    /// </summary>
    Task Cleanup();
  }
}
