using Raven.Client.Documents.Indexes;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Database.Indexes
{
  public class zero_Spaces : ZeroIndex<SpaceContent>
  {
    protected override void Create()
    {
      Map = items => items.Select(item => new
      {
        Name = item.Name,
        IsActive = item.IsActive,
        SpaceAlias = item.SpaceAlias,
        CreatedDate = item.CreatedDate
      });

      Index(x => x.Name, FieldIndexing.Search);
    }
  }
}
