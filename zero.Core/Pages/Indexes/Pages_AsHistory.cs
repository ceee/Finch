using Raven.Client.Documents.Indexes;

namespace zero.Pages;

public class Pages_AsHistory : ZeroIndex<Page, Pages_AsHistory.Result>
{
  public class Result : ZeroIdEntity, ISupportsPersistence
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
