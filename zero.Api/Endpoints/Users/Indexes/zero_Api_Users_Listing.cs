using Raven.Client.Documents.Indexes;

namespace zero.Api.Endpoints.Users;

public class zero_Api_Users_Listing : ZeroIndex<ZeroUser>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new
    {
      Name = item.Name,
      CreatedDate = item.CreatedDate
    });

    Index(x => x.Name, FieldIndexing.Search);
  }
}