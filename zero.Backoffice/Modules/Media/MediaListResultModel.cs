using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Web.Models
{
  public class MediaListResultModel : Paged<MediaListModel>
  {
    public IEnumerable<MediaListModel> Folders { get; private set; }

    public IEnumerable<MediaFolder> FolderHierarchy { get; private set; }

    public MediaFolder Folder { get; private set; }

    public MediaListResultModel(Paged<MediaListModel> items, IEnumerable<MediaListModel> folders) : base(items.Items, items.TotalItems, items.Page, items.PageSize)
    {
      Folders = folders;
    }

    public MediaListResultModel(Paged<MediaListModel> items, IEnumerable<MediaListModel> folders, MediaFolder currentFolder, IEnumerable<MediaFolder> hierarchy) : this(items, folders)
    {
      Folder = currentFolder;
      FolderHierarchy = hierarchy;
    }
  }
}
