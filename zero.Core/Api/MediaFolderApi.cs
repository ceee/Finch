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
  public class MediaFolderApi : IMediaFolderApi
  {
    protected IAppAwareBackofficeStore Backoffice { get; private set; }


    public MediaFolderApi(IAppAwareBackofficeStore backoffice)
    {
      Backoffice = backoffice;
    }


    /// <inheritdoc />
    public async Task<MediaFolder> GetById(string id)
    {
      return await Backoffice.GetById<MediaFolder>(id);
    }


    /// <inheritdoc />
    public async Task<IList<MediaFolder>> GetAll(string parentId = null)
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
    public async Task<IList<TreeItem>> GetAllAsTree(string parentId = null)
    {
      List<TreeItem> result = new List<TreeItem>();

      using (IAsyncDocumentSession session = Backoffice.Raven.OpenAsyncSession())
      {
        IList<MediaFolder> items = await session.Query<MediaFolder>()
          .ForApp(Backoffice.AppId)
          .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty(), x => x.ParentId == null)
          .OrderByDescending(x => x.Name)
          .ToListAsync();

        foreach (MediaFolder folder in items)
        {
          result.Add(new TreeItem()
          {
            Id = folder.Id,
            Name = folder.Name,
            HasChildren = true,
            ParentId = folder.ParentId,
            Sort = folder.Sort,
            Icon = "fth-folder"
          });
        } 
      }

      if (parentId.IsNullOrEmpty())
      {
        result.Add(new TreeItem()
        {
          Id = "recyclebin",
          ParentId = null,
          Sort = 99999,
          Name = "@page.recyclebin.name",
          Icon = "fth-trash",
          HasChildren = false
        });
      }

      return result;
    }


    /// <inheritdoc />
    public async Task<IList<MediaFolder>> GetHierarchy(string id)
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
    public async Task<EntityResult<MediaFolder>> Save(MediaFolder model)
    {
      return await Backoffice.Save(model, new MediaFolderValidator());
    }


    /// <inheritdoc />
    public async Task<EntityResult<MediaFolder>> Delete(string id)
    {
      return await Backoffice.DeleteById<MediaFolder>(id);
    }
  }


  public interface IMediaFolderApi
  {
    /// <summary>
    /// Get application by Id
    /// </summary>
    Task<MediaFolder> GetById(string id);

    /// <summary>
    /// Get hierarchy for a folder
    /// </summary>
    Task<IList<MediaFolder>> GetHierarchy(string id);

    /// <summary>
    /// Get all folders with the specified parent or on root
    /// </summary>
    Task<IList<MediaFolder>> GetAll(string parentId = null);

    /// <summary>
    /// Get all folders with the specified parent or on root for tree output
    /// </summary>
    Task<IList<TreeItem>> GetAllAsTree(string parentId = null);

    /// <summary>
    /// Creates or updates a folder
    /// </summary>
    Task<EntityResult<MediaFolder>> Save(MediaFolder model);

    /// <summary>
    /// Deletes a folder
    /// </summary>
    Task<EntityResult<MediaFolder>> Delete(string id);
  }
}
