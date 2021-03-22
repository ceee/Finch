using Raven.Client.Documents.Indexes;
using System;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class Media_ByParent : AbstractMultiMapIndexCreationTask<MediaListItem>
  {
    public Media_ByParent()
    {
      AddMap<IMediaFolder>(items => items.Select(item => new MediaListItem
      {
        Id = item.Id,
        ParentId = item.ParentId,
        CreatedDate = item.CreatedDate,
        IsFolder = true,
        Name = item.Name,
        Image = null,
        Children = 0,
        Size = 0,
        IsShared = item.Blueprint != null,
        AspectRatio = 0
      }));

      AddMap<IMedia>(items => items.Select(item => new MediaListItem
      {
        Id = item.Id,
        ParentId = item.FolderId,
        CreatedDate = item.CreatedDate,
        IsFolder = false,
        Name = item.Name,
        Image = item.PreviewSource,
        Children = 0,
        Size = item.Size,
        IsShared = item.Blueprint != null,
        AspectRatio = item.ImageMeta != null ? (float)item.ImageMeta.Width / item.ImageMeta.Height : 0
      }));

      StoreAllFields(FieldStorage.Yes);
      Index(x => x.ParentId, FieldIndexing.Exact);
    }
  }
}
