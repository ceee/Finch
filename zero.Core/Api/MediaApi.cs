using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Api
{
  public class MediaApi : IMediaApi
  {
    protected IAppAwareBackofficeStore Backoffice { get; private set; }


    public MediaApi(IAppAwareBackofficeStore backoffice)
    {
      Backoffice = backoffice;
    }


    /// <inheritdoc />
    public async Task<Media> GetById(string id)
    {
      return await Backoffice.GetById<Media>(id);
    }


    /// <inheritdoc />
    public async Task<ListResult<Media>> GetByQuery(MediaListQuery query)
    {
      query.SearchFor(entity => entity.Name);

      using (IAsyncDocumentSession session = Backoffice.Raven.OpenAsyncSession())
      {
        return await session.Query<Media>()
          .ForApp(Backoffice.AppId)
          .WhereIf(x => x.FolderId == query.Filter.FolderId, query.Filter != null && !query.Filter.FolderId.IsNullOrEmpty())
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<Media>> Save(Media model)
    {
      return await Backoffice.Save(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Media>> Delete(string id)
    {
      return await Backoffice.DeleteById<Media>(id);
    }


    /// <inheritdoc />
    public async Task Cleanup()
    {
      await Task.Delay(0);
    }
  }


  public interface IMediaApi
  {
    /// <summary>
    /// Get application by Id
    /// </summary>
    Task<Media> GetById(string id);

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
