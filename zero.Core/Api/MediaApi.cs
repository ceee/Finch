using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Api
{
  public class MediaApi : ApiBase, IMediaApi
  {
    public MediaApi(IDocumentStore raven, IMediaUpload media) : base(raven, media) { }


    /// <inheritdoc />
    public async Task<IList<MediaFolder>> GetFolders(string parentId = null)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<MediaFolder>()
          .OrderByDescending(x => x.Name)
          .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty())
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<MediaFolder>> SaveFolder(MediaFolder model)
    {
      return await Save(model, new MediaFolderValidator());
    }


    /// <inheritdoc />
    public async Task<EntityResult<MediaFolder>> DeleteFolder(string id)
    {
      return await DeleteById<MediaFolder>(id);
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
