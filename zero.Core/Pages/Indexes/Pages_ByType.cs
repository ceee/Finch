using Raven.Client.Documents.Indexes;
using System.Linq;

namespace zero;

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