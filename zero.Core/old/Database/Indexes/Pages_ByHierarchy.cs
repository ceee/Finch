using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class Pages_ByHierarchy : ZeroIndex<Page, Pages_ByHierarchy.Result>
  {
    public class Result : ZeroIdEntity, IZeroDbConventions
    {
      public string Name { get; set; }

      public List<PathResult> Path { get; set; } = new List<PathResult>();

      public string[] PathIds { get; set; } = Array.Empty<string>();
    }


    public class PathResult
    {
      public string Id { get; set; }

      public string Name { get; set; }
    }


    protected override void Create()
    {
      Map = items => items
        .Select(item => new
        {
          Item = item,
          Path = Recurse(item, x => LoadDocument<Page>(x.ParentId)).Where(x => x != null && x.Id != null && x.Id != item.Id).Reverse()
        })
        .Select(item => new Result
        {
          Id = item.Item.Id,
          Name = item.Item.Name,
          Path = item.Path.Select(current => new PathResult() { Id = current.Id, Name = current.Name }).ToList(),
          PathIds = item.Path.Select(current => current.Id).ToArray()
        });

      StoreAllFields(FieldStorage.Yes);
      Index("PathIds", FieldIndexing.Exact);
      //Index(x => x.ChannelId, FieldIndexing.Exact);
    }
  }
}
