using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database.Indexes;

namespace zero.Core.Collections
{
  public class MediaFolderCollection : EntityStore<MediaFolder>, IMediaFolderCollection
  {
    public MediaFolderCollection(IStoreContext<MediaFolder> context) : base(context)
    {
      Options = new(true);
    }


    /// <inheritdoc />
    public override Task<EntityResult<MediaFolder>> Save(MediaFolder model)
    {
      model.IsActive = true;
      return base.Save(model);
    }


    /// <inheritdoc />
    public async Task<List<MediaFolder>> LoadByParent(string parentId = null)
    {
      return await Session.Query<MediaFolder>()
        .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty(), x => x.ParentId == null)
        .OrderByDescending(x => x.Name)
        .ToListAsync();
    }


    /// <inheritdoc />
    public async Task<EntityResult<MediaFolder>> Move(string id, string parentId)
    {
      MediaFolder model = await Load(id);
      MediaFolder parent = await Load(parentId);

      if (model == null || (!parentId.IsNullOrEmpty() && parent == null))
      {
        return EntityResult<MediaFolder>.Fail("@errors.idnotfound");
      }

      model.ParentId = parent?.Id;

      return await Save(model);
    }


    /// <inheritdoc />
    public async Task<List<TreeItem>> LoadAsTree(string parentId = null, string activeId = null)
    {
      List<TreeItem> items = new();
      string[] openIds = Array.Empty<string>();

      IList<MediaFolder> folders = await Session.Query<MediaFolder>()
        .WhereIf(x => x.ParentId == parentId, !parentId.IsNullOrEmpty(), x => x.ParentId == null)
        .OrderByDescending(x => x.CreatedDate).ThenBy(x => x.Name)
        .ToListAsync();


      // get hierarchy so we know if we should set the folder to open
      if (!activeId.IsNullOrEmpty())
      {
        MediaFolder_ByHierarchy.Result result = await Session.Query<MediaFolder_ByHierarchy.Result, MediaFolder_ByHierarchy>()
          .ProjectInto<MediaFolder_ByHierarchy.Result>()
          .Include<MediaFolder_ByHierarchy.Result, MediaFolder>(x => x.Path.Select(p => p.Id))
          .FirstOrDefaultAsync(x => x.Id == activeId);

        if (result != null)
        {
          openIds = result.Path.Select(x => x.Id).ToArray();
        }
      }


      // get children for all folders
      string[] folderIds = folders.Select(x => x.Id).ToArray();

      IList<MediaFolders_WithChildren.Result> children = await Session.Query<MediaFolders_WithChildren.Result, MediaFolders_WithChildren>()
        .ProjectInto<MediaFolders_WithChildren.Result>()
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
            Icon = "fth-minus-circle color-yellow",
            Name = "Inactive"
          } : null
        });
      }

      return items;
    }


    /// <inheritdoc />
    public async Task<List<MediaFolder>> GetHierarchy(string id)
    {
      MediaFolder_ByHierarchy.Result result = await Session.Query<MediaFolder_ByHierarchy.Result, MediaFolder_ByHierarchy>()
        .ProjectInto<MediaFolder_ByHierarchy.Result>()
        .Include<MediaFolder_ByHierarchy.Result, MediaFolder>(x => x.Path.Select(p => p.Id))
        .FirstOrDefaultAsync(x => x.Id == id);

      if (result == null)
      {
        return new List<MediaFolder>();
      }

      List<string> ids = result.Path.Select(x => x.Id).ToList();
      ids.Add(id);

      return (await Session.LoadAsync<MediaFolder>(ids)).Select(x => x.Value).ToList();
    }


    /// <inheritdoc />
    protected override void ValidationRules(ZeroValidator<MediaFolder> validator)
    {
      validator.RuleFor(x => x.Name).Length(2, 80);
      validator.RuleFor(x => x.IsActive).Equal(true);
      validator.RuleFor(x => x.ParentId).Exists(Context.Store);
    }
  }


  public interface IMediaFolderCollection : IEntityStore<MediaFolder>
  {
    /// <summary>
    /// Get hierarchy for a folder
    /// </summary>
    Task<List<MediaFolder>> GetHierarchy(string id);

    /// <summary>
    /// Get all folders with the specified parent or on root
    /// </summary>
    Task<List<MediaFolder>> LoadByParent(string parentId = null);

    /// <summary>
    /// Get all folders with the specified parent or on root for tree output
    /// </summary>
    Task<List<TreeItem>> LoadAsTree(string parentId = null, string activeId = null);

    /// <summary>
    /// Move a folder to a new parent
    /// </summary>
    Task<EntityResult<MediaFolder>> Move(string id, string parentId);
  }
}
