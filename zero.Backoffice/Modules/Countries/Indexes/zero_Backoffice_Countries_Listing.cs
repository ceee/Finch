using Raven.Client.Documents.Indexes;

namespace zero.Backoffice.Modules.Countries;

public class zero_Backoffice_Countries_Listing : ZeroIndex<Country>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new
    {
      Name = item.Name,
      IsPreferred = item.IsPreferred
    });

    Index(x => x.Name, FieldIndexing.Search);
  }
}