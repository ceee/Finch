using Raven.Client.Documents.Indexes;

namespace zero.Api.Endpoints.Media;

public class zero_Api_Media_Listing : ZeroIndex<zero.Media.Media>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new
    {
      Name = item.Name,
      ParentId = item.ParentId,
      CreatedDate = item.CreatedDate,
      Type = item.Type
    });

    Index(x => x.Name, FieldIndexing.Search);
    Index(x => x.ParentId, FieldIndexing.Exact);
    Index(x => x.CreatedDate, FieldIndexing.Exact);
    Index(x => x.Type, FieldIndexing.Exact);
  }
}