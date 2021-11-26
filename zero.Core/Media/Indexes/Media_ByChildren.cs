using Raven.Client.Documents.Indexes;

namespace zero.Media;

public class Media_ByChildren : ZeroMultiMapIndex<Media_ByChildren.Result>
{
  public class Result : ZeroIdEntity, IZeroDbConventions
  {
    public string ParentId { get; set; }

    public int ChildrenCount { get; set; }

    public string[] ChildrenIds { get; set; }
  }


  protected override void Create()
  {
    AddMap<Media>(items => items.Select(item => new Result()
    {
      Id = item.Id,
      ParentId = item.ParentId,
      ChildrenCount = 1,
      ChildrenIds = new string[] { }
    }));

    Reduce = results => results.GroupBy(x => new { x.ParentId }).Select(group => new Result()
    {
      Id = null,
      ParentId = group.Key.ParentId,
      ChildrenCount = group.Sum(x => x.ChildrenCount),
      ChildrenIds = group.Select(x => x.Id).ToArray()
    });

    StoreAllFields(FieldStorage.Yes);
    Index(x => x.ParentId, FieldIndexing.Exact);
  }
}