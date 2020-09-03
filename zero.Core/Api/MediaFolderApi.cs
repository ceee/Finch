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
    public async Task<IMediaFolder> GetById(string id)
    {
      return await GetById<IMediaFolder>(id);
    }


    /// <inheritdoc />
    public async Task<IList<IMediaFolder>> GetAll(string parentId = null)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<IMediaFolder>()
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
        IList<IMediaFolder> folders = await session.Query<IMediaFolder>()
          .Scope(Scope)
          .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty(), x => x.ParentId == null)
          .OrderByDescending(x => x.CreatedDate).ThenBy(x => x.Name)
          .ToListAsync();


        // get hierarchy so we know if we should set the folder to open
        if (!activeId.IsNullOrEmpty())
        {
          MediaFolder_ByHierarchy.Result result = await session.Query<MediaFolder_ByHierarchy.Result, MediaFolder_ByHierarchy>()
            .ProjectInto<MediaFolder_ByHierarchy.Result>()
            .Include<MediaFolder_ByHierarchy.Result, IMediaFolder>(x => x.Path.Select(p => p.Id))
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


        foreach (IMediaFolder folder in folders)
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
              Icon = "fth-minus-circle color-yellow",
              Name = "Inactive"
            } : null
          });
        } 
      }

      //if (parentId.IsNullOrEmpty())
      //{
      //  items.Add(new TreeItem()
      //  {
      //    Id = "recyclebin",
      //    ParentId = null,
      //    Sort = 99999,
      //    Name = "@recyclebin.name",
      //    Icon = "fth-trash",
      //    HasChildren = false
      //  });
      //}

      return items;
    }


    /// <inheritdoc />
    public async Task<IList<IMediaFolder>> GetHierarchy(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        MediaFolder_ByHierarchy.Result result = await session.Query<MediaFolder_ByHierarchy.Result, MediaFolder_ByHierarchy>()
          .ProjectInto<MediaFolder_ByHierarchy.Result>()
          .Include<MediaFolder_ByHierarchy.Result, IMediaFolder>(x => x.Path.Select(p => p.Id))
          .Scope(Scope)
          .FirstOrDefaultAsync(x => x.Id == id);

        if (result == null)
        {
          return new List<IMediaFolder>();
        }

        List<string> ids = result.Path.Select(x => x.Id).ToList();
        ids.Add(id);

        return (await session.LoadAsync<IMediaFolder>(ids)).Select(x => x.Value).ToList();
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<IMediaFolder>> Save(IMediaFolder model)
    {
      model.IsActive = true;
      return await SaveModel(model, new MediaFolderValidator());
    }


    /// <inheritdoc />
    public async Task<EntityResult<IMediaFolder>> Move(string id, string parentId)
    {
      IMediaFolder model = await GetById<IMediaFolder>(id);
      IMediaFolder parent = await GetById<IMediaFolder>(parentId);

      if (model == null || (!parentId.IsNullOrEmpty() && parent == null))
      {
        return EntityResult<IMediaFolder>.Fail("@errors.idnotfound");
      }

      model.ParentId = parent?.Id;

      return await Save(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IMediaFolder>> Delete(string id)
    {
      return await DeleteById<IMediaFolder>(id);
    }
  }


  public interface IMediaFolderApi : IAppAwareBackofficeApi
  {
    /// <summary>
    /// Get application by Id
    /// </summary>
    Task<IMediaFolder> GetById(string id);

    /// <summary>
    /// Get hierarchy for a folder
    /// </summary>
    Task<IList<IMediaFolder>> GetHierarchy(string id);

    /// <summary>
    /// Get all folders with the specified parent or on root
    /// </summary>
    Task<IList<IMediaFolder>> GetAll(string parentId = null);

    /// <summary>
    /// Get all folders with the specified parent or on root for tree output
    /// </summary>
    Task<IList<TreeItem>> GetAllAsTree(string parentId = null, string activeId = null);

    /// <summary>
    /// Creates or updates a folder
    /// </summary>
    Task<EntityResult<IMediaFolder>> Save(IMediaFolder model);

    /// <summary>
    /// Move a folder to a new parent
    /// </summary>
    Task<EntityResult<IMediaFolder>> Move(string id, string parentId);

    /// <summary>
    /// Deletes a folder
    /// </summary>
    Task<EntityResult<IMediaFolder>> Delete(string id);
  }
}
