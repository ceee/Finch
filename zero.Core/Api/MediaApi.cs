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
    public async Task<IList<MediaFolder>> GetFolders(string parentId = null)
    {
      using (IAsyncDocumentSession session = Backoffice.Raven.OpenAsyncSession())
      {
        return await session.Query<MediaFolder>()
          .ForApp(Backoffice.AppId)
          .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty(), x => x.ParentId == null)
          .OrderByDescending(x => x.Name)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<IList<MediaFolder>> GetFolderHierarchy(string id)
    {
      using (IAsyncDocumentSession session = Backoffice.Raven.OpenAsyncSession())
      {
        MediaFolder_ByHierarchy.Result result = await session.Query<MediaFolder_ByHierarchy.Result, MediaFolder_ByHierarchy>()
          .ProjectInto<MediaFolder_ByHierarchy.Result>()
          .Include<MediaFolder_ByHierarchy.Result, MediaFolder>(x => x.Path.Select(p => p.Id))
          .ForApp(Backoffice.AppId)
          .FirstOrDefaultAsync(x => x.Id == id);

        if (result == null)
        {
          return new List<MediaFolder>();
        }

        return (await session.LoadAsync<MediaFolder>(result.Path.Select(x => x.Id))).Select(x => x.Value).ToList();
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<MediaFolder>> SaveFolder(MediaFolder model)
    {
      return await Backoffice.Save(model, new MediaFolderValidator());
    }


    /// <inheritdoc />
    public async Task<EntityResult<MediaFolder>> DeleteFolder(string id)
    {
      return await Backoffice.DeleteById<MediaFolder>(id);
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
    /// Get hierarchy for a folder
    /// </summary>
    Task<IList<MediaFolder>> GetFolderHierarchy(string id);

    /// <summary>
    /// Get all folders with the specified parent or on root
    /// </summary>
    Task<IList<MediaFolder>> GetFolders(string parentId = null);

    /// <summary>
    /// Creates or updates a folder
    /// </summary>
    Task<EntityResult<MediaFolder>> SaveFolder(MediaFolder model);

    /// <summary>
    /// Deletes a folder
    /// </summary>
    Task<EntityResult<MediaFolder>> DeleteFolder(string id);

    /// <summary>
    /// Clean-up all media based on stored database information
    /// </summary>
    Task Cleanup();
  }
}
