using Raven.Client.Documents.Indexes;
using System.Linq;
using zero.Core.Entities;
using zero.Core.Routing;

namespace zero.Core.Database.Indexes
{
  public class Pages_WithUrl : AbstractIndexCreationTask<IPage, Pages_WithUrl.Result>
  {
    public class Result
    {
      public string Id { get; set; }

      public string AppId { get; set; }

      public string ParentId { get; set; }

      public string Name { get; set; }

      public string Url { get; set; }
    }


    public Pages_WithUrl()
    {
      Map = items => items.Select(item => new Result()
      {
        Id = item.Id,
        AppId = item.AppId,
        Name = item.Name,
        ParentId = item.ParentId,
        Url = LoadDocument<IRoute>("routes." + item.Hash).Url
      });

      Index(x => x.Id, FieldIndexing.Exact);
      Index(x => x.AppId, FieldIndexing.Exact);
      Index(x => x.ParentId, FieldIndexing.Exact);
      StoreAllFields(FieldStorage.Yes);
    }
  }
}
