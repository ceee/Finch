using Raven.Client.Documents.Indexes;
using System;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class MediaFolders_WithChildren : AbstractIndexCreationTask<MediaFolder, MediaFolders_WithChildren.Result>
  {
    public class Result : ZeroIdEntity
    {
      public string ParentId { get; set; }

      public string Name { get; set; }

      public string[] ChildrenIds { get; set; }
    }


    public MediaFolders_WithChildren()
    {
      Map = items => items.Where(x => x.ParentId != null).Select(item => new Result
      {
        Id = item.Id,
        ParentId = item.ParentId,
        Name = item.Name,
        ChildrenIds = new string[] { }
      });

      Reduce = results => results.GroupBy(x => new { x.ParentId }).Select(group => new Result()
      {
        Id = group.Key.ParentId,
        ParentId = group.Key.ParentId,
        Name = String.Empty,
        ChildrenIds = group.Select(x => x.Id).ToArray()
      });

      StoreAllFields(FieldStorage.Yes);
      //Index(x => x.ChannelId, FieldIndexing.Exact);
    }
  }
}
