//using Raven.Client.Documents.Indexes;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using zero.Core.Entities;

//namespace zero.Core.Database.Indexes
//{
//  public class TreeEntity_ByPath : AbstractIndexCreationTask<ITreeEntity, TreeEntity_ByPath.Result>
//  {
//    public class Result : IZeroIdEntity, IZeroDbConventions
//    {
//      public string Id { get; set; }

//      public string Name { get; set; }

//      public List<PathResult> Path { get; set; } = new List<PathResult>();

//      public List<string> PathIds { get; set; } = new List<string>();
//    }


//    public class PathResult
//    {
//      public string Id { get; set; }

//      public string Name { get; set; }
//    }


//    public TreeEntity_ByPath()
//    {
//      Map = items => items.Select(item => new Result
//      {
//        Id = item.Id,
//        Name = item.Name,
//        Path = Recurse(item, x => LoadDocument<ITreeEntity>(x.ParentId))
//                .Where(x => x != null && x.Id != null && x.Id != item.Id)
//                .Reverse()
//                .Select(current => new PathResult()
//                {
//                  Id = current.Id,
//                  Name = current.Name
//                })
//                .ToList(),
//        PathIds = Recurse(item, x => LoadDocument<ITreeEntity>(x.ParentId))
//                  .Where(x => x != null && x.Id != null && x.Id != item.Id)
//                  .Reverse()
//                  .Select(current => current.Id)
//                  .ToList()
//      });

//      StoreAllFields(FieldStorage.Yes);
//      Index("PathIds", FieldIndexing.Exact);
//      //Index(x => x.ChannelId, FieldIndexing.Exact);
//    }
//  }
//}
