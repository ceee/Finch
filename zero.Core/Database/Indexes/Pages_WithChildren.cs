using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class Pages_WithChildren : AbstractIndexCreationTask<Page, Pages_WithChildren.Result>
  {
    public class Result : IZeroIdEntity, IAppAwareEntity, IZeroDbConventions
    {
      public string Id { get; set; }

      public string ParentId { get; set; }

      public string AppId { get; set; }

      public string Name { get; set; }

      public string[] ChildrenIds { get; set; }
    }


    public Pages_WithChildren()
    {
      Map = items => items.Where(x => x.ParentId != null).Select(item => new Result
      {
        Id = item.Id,
        ParentId = item.ParentId,
        Name = item.Name,
        AppId = item.AppId,
        ChildrenIds = new string[] { }
      });

      Reduce = results => results.GroupBy(x => new { x.ParentId, x.AppId }).Select(group => new Result()
      {
        Id = group.Key.ParentId,
        ParentId = group.Key.ParentId,
        Name = String.Empty,
        AppId = group.Key.AppId,
        ChildrenIds = group.Select(x => x.Id).ToArray()
      });

      StoreAllFields(FieldStorage.Yes);
      //Index(x => x.ChannelId, FieldIndexing.Exact);
    }
  }
}
