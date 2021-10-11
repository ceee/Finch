using Raven.Client.Documents.Indexes;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
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
}
