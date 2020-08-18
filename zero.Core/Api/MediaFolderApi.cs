using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
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
  public class MediaFolderApi : AppAwareBackofficeApi, IMediaFolderApi
  {
    public MediaFolderApi(IBackofficeStore store) : base(store) { }


    /// <inheritdoc />
    public async Task<MediaFolder> GetById(string id)
    {
      return await GetById<MediaFolder>(id);
    }


    /// <inheritdoc />
    public async Task<IList<MediaFolder>> GetAll(string parentId = null)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<MediaFolder>()
          .Scope(Scope)
          .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty(), x => x.ParentId == null)
          .OrderByDescending(x => x.Name)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<IList<TreeItem>> GetAllAsTree(string parentId = null, string activeId = null)
    {
      List<TreeItem> items = new List<TreeItem>();
      string[] openIds = new string[0] { };

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        IList<MediaFolder> folders = await session.Query<MediaFolder>()
          .Scope(Scope)
          .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty(), x => x.ParentId == null)
          .OrderByDescending(x => x.Name)
          .ToListAsync();


        // get hierarchy so we know if we should set the folder to open
        if (!activeId.IsNullOrEmpty())
        {
          MediaFolder_ByHierarchy.Result result = await session.Query<MediaFolder_ByHierarchy.Result, MediaFolder_ByHierarchy>()
            .ProjectInto<MediaFolder_ByHierarchy.Result>()
            .Include<MediaFolder_ByHierarchy.Result, MediaFolder>(x => x.Path.Select(p => p.Id))
            .Scope(Scope)
            .FirstOrDefaultAsync(x => x.Id == activeId);

          if (result != null)
          {
            openIds = result.Path.Select(x => x.Id).ToArray();
          }
        }


        // get children for all folders
        string[] folderIds = folders.Select(x => x.Id).ToArray();

        IList<MediaFolders_WithChildren.Result> children = await session.Query<MediaFolders_WithChildren.Result, MediaFolders_WithChildren>()
          .ProjectInto<MediaFolders_WithChildren.Result>()
          .Scope(Scope)
          .Where(x => x.Id.In(folderIds))
          .ToListAsync();


        foreach (MediaFolder folder in folders)
        {
          int childCount = children.Count(x => x.Id == folder.Id);

          items.Add(new TreeItem()
          {
            Id = folder.Id,
            Name = folder.Name,
            HasChildren = childCount > 0,
            ChildCount = childCount,
            ParentId = folder.ParentId,
            Sort = folder.Sort,
            Icon = "fth-folder",
            IsOpen = openIds.Contains(folder.Id),
            IsInactive = !folder.IsActive,
            HasActions = true,
            Modifier = !folder.IsActive ? new TreeItemModifier()
            {
              Icon = "fth-minus-circle color-orange",
              Name = "Inactive"
            } : null
          });
        } 
      }

      if (parentId.IsNullOrEmpty())
      {
        items.Add(new TreeItem()
        {
          Id = "recyclebin",
          ParentId = null,
          Sort = 99999,
          Name = "@page.recyclebin.name",
          Icon = "fth-trash",
          HasChildren = false
        });
      }

      return items;
    }


    /// <inheritdoc />
    public async Task<IList<MediaFolder>> GetHierarchy(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        MediaFolder_ByHierarchy.Result result = await session.Query<MediaFolder_ByHierarchy.Result, MediaFolder_ByHierarchy>()
          .ProjectInto<MediaFolder_ByHierarchy.Result>()
          .Include<MediaFolder_ByHierarchy.Result, MediaFolder>(x => x.Path.Select(p => p.Id))
          .Scope(Scope)
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
      model.IsActive = true;
      return await SaveModel(model, new MediaFolderValidator());
    }


    /// <inheritdoc />
    public async Task<EntityResult<MediaFolder>> Delete(string id)
    {
      return await DeleteById<MediaFolder>(id);
    }
  }


  public interface IMediaFolderApi : IAppAwareBackofficeApi
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
    Task<IList<TreeItem>> GetAllAsTree(string parentId = null, string activeId = null);

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
