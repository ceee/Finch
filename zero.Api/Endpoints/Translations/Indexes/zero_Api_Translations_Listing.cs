using Raven.Client.Documents.Indexes;

namespace zero.Api.Endpoints.Translations;

public class zero_Api_Translations_Listing : ZeroIndex<Translation>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new
    {
      Name = item.Name,
      Value = item.Value,
      Key = item.Key,
      CreatedDate = item.CreatedDate
    });

    Index(x => x.Name, FieldIndexing.Search);
    Index(x => x.Key, FieldIndexing.Search);
    Index(x => x.Value, FieldIndexing.Search);
  }
}