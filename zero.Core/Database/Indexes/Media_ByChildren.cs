using Raven.Client.Documents.Indexes;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class Media_ByChildren : AbstractMultiMapIndexCreationTask<Media_ByChildren.Result>
  {
    public class Result : ZeroIdEntity, IZeroDbConventions
    {
      public string ParentId { get; set; }

      public int ChildrenCount { get; set; }

      public string[] ChildrenIds { get; set; }
    }


    public Media_ByChildren()
    {
      AddMap<Media>(items => items.Select(item => new Result()
      {
        Id = item.Id,
        ParentId = item.FolderId,
        ChildrenCount = 1,
        ChildrenIds = new string[] { }
      }));

      AddMap<MediaFolder>(items => items.Select(item => new Result()
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
}
