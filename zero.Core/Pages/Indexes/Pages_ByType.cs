using Raven.Client.Documents.Indexes;

namespace zero.Pages;

public class Pages_ByType : ZeroIndex<Page>
{
  protected override void Create()
  {
    Map = items => items.Select(item => new
    {
      PageTypeAlias = item.PageTypeAlias
    });

    Index(x => x.PageTypeAlias, FieldIndexing.Exact);
  }
}