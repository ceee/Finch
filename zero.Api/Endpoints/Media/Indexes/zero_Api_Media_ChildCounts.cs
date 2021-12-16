using Raven.Client.Documents.Indexes;

namespace zero.Api.Endpoints.Media;

public class zero_Api_Media_ChildCounts : ZeroIndex<zero.Media.Media, zero_Api_Media_ChildCounts.Result>
{
  public class Result : ZeroIdEntity
  {
    public int ChildCount { get; set; }

    public int ChildFolderCount { get; set; }
  }

  protected override void Create()
  {
    Map = items => items.Where(x => x.ParentId != null).Select(item => new Result
    {
      Id = item.ParentId,
      ChildCount = 1,
      ChildFolderCount = item.IsFolder ? 1 : 0
    });

    Reduce = results => results.GroupBy(x => new { x.Id }).Select(group => new Result()
    {
      Id = group.Key.Id,
      ChildCount = group.Sum(x => x.ChildCount),
      ChildFolderCount = group.Sum(x => x.ChildFolderCount)
    });

    StoreAllFields(FieldStorage.Yes);
  }
}