using Raven.Client.Documents.Indexes;

namespace zero.Api.Endpoints.Pages;

public class zero_Api_Pages_Listing : ZeroIndex<Page>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new
    {
      Name = item.Name,
      ParentId = item.ParentId,
      CreatedDate = item.CreatedDate,
      Sort = item.Sort
    });

    Index(x => x.Name, FieldIndexing.Search);
    Index(x => x.ParentId, FieldIndexing.Exact);
    Index(x => x.CreatedDate, FieldIndexing.Exact);
    Index(x => x.Sort, FieldIndexing.Exact);
  }
}