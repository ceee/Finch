using Raven.Client.Documents.Indexes;

namespace zero.Api.Endpoints.Media;

public class zero_Api_Media_Listing : ZeroIndex<zero.Media.Media>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new
    {
      Name = item.Name,
      ParentId = item.ParentId
    });

    Index(x => x.Name, FieldIndexing.Search);
    Index(x => x.ParentId, FieldIndexing.Exact);
  }
}