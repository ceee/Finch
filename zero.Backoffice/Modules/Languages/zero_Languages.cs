using Raven.Client.Documents.Indexes;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class zero_Languages : ZeroIndex<Language>
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
}
