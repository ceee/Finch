using Raven.Client.Documents.Indexes;
using System;
using System.Linq;

namespace zero;

public class Pages_AsHistory : ZeroIndex<Page, Pages_AsHistory.Result>
{
  public class Result : ZeroIdEntity, IZeroDbConventions
  {
    public DateTimeOffset LastModified { get; set; }
  }


  protected override void Create()
  {
    Map = items => items
      .Select(x => new Result()
      {
        Id = x.Id,
        LastModified = x.LastModifiedDate
      });

    StoreAllFields(FieldStorage.Yes);
    //Index(x => x.ChannelId, FieldIndexing.Exact);
  }
}
