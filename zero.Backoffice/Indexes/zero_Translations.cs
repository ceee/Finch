using Raven.Client.Documents.Indexes;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class zero_Translations : ZeroIndex<Translation>
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
  }
}
