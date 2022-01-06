using Raven.Client.Documents.Indexes;

namespace zero.Pages;

public class zero_Pages_ByType : ZeroIndex<Page>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new
    {
      PageTypeAlias = item.Flavor
    });

    Index(x => x.Flavor, FieldIndexing.Exact);
  }
}