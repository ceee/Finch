using Raven.Client.Documents.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class MediaFolder_ByHierarchy : AbstractIndexCreationTask<IMediaFolder, MediaFolder_ByHierarchy.Result>
  {
    public class Result : IZeroIdEntity, IZeroDbConventions
    {
      public string Id { get; set; }

      public string Name { get; set; }

      public List<PathResult> Path { get; set; } = new List<PathResult>();
    }


    public class PathResult
    {
      public string Id { get; set; }

      public string Name { get; set; }
    }


    public MediaFolder_ByHierarchy()
    {
      Map = items => items.Select(item => new Result
      {
        Id = item.Id,
        Name = item.Name,
        Path = Recurse(item, x => LoadDocument<IMediaFolder>(x.ParentId))
                .Where(x => x != null && x.Id != null && x.Id != item.Id)
                .Reverse()
                .Select(current => new PathResult()
                {
                  Id = current.Id,
                  Name = current.Name
                })
                .ToList()
      });

      StoreAllFields(FieldStorage.Yes);
      //Index(x => x.ChannelId, FieldIndexing.Exact);
    }
  }
}
