using Raven.Client.Documents.Indexes;

namespace zero.Backoffice.Modules;

public class zero_Backoffice_Translations : ZeroIndex<Translation>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new
    {
      Key = item.Key,
      Value = item.Value,
      CreatedDate = item.CreatedDate
    });

    Index(x => x.Key, FieldIndexing.Search);
    Index(x => x.Value, FieldIndexing.Search);
  }
};