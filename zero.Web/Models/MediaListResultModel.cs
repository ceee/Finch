using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Web.Models
{
  public class MediaListResultModel : ListResult<MediaListModel>
  {
    public IEnumerable<MediaListModel> Folders { get; private set; }

    public MediaFolder Folder { get; private set; }

    public MediaListResultModel(ListResult<MediaListModel> items, IEnumerable<MediaListModel> folders) : base(items.Items, items.TotalItems, items.Page, items.PageSize)
    {
      Folders = folders;
      Statistics = items.Statistics;
    }

    public MediaListResultModel(ListResult<MediaListModel> items, IEnumerable<MediaListModel> folders, MediaFolder currentFolder) : this(items, folders)
    {
      Folder = currentFolder;
    }
  }
}
