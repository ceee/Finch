using Raven.Client.Documents.Indexes;

namespace zero.Media;

public class Media_ByParent : ZeroMultiMapIndex<MediaListItem>
{
  protected override void Create()
  {
    AddMap<MediaFolder>(items => items.Select(item => new MediaListItem
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

    AddMap<Media>(items => items.Select(item => new MediaListItem
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