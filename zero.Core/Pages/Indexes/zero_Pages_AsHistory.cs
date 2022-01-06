using Raven.Client.Documents.Indexes;

namespace zero.Pages;

public class zero_Pages_AsHistory : ZeroIndex<Page, zero_Pages_AsHistory.Result>
{
  public class Result : ZeroIdEntity, ISupportsDbConventions
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
