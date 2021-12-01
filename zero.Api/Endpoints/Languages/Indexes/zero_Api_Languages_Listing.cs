using Raven.Client.Documents.Indexes;

namespace zero.Api.Endpoints.Languages;

public class zero_Api_Languages_Listing : ZeroIndex<Language>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new
    {
      Name = item.Name,
      CreatedDate = item.CreatedDate,
      IsDefault = item.IsDefault
    });

    Index(x => x.Name, FieldIndexing.Search);
  }
}