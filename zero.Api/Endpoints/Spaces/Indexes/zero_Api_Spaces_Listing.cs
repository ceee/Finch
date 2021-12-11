using Raven.Client.Documents.Indexes;

namespace zero.Api.Endpoints.Spaces;

public class zero_Api_Spaces_Listing : ZeroIndex<Space>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new
    {
      Name = item.Name,
      IsActive = item.IsActive,
      Flavor = item.Flavor,
      CreatedDate = item.CreatedDate
    });

    Index(x => x.Name, FieldIndexing.Search);
  }
}