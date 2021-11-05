using System.Linq;
using zero.Core.Routing;

namespace zero.Core.Database.Indexes
{
  public class Routes_ByDependencies : ZeroIndex<Route>
  {
    protected override void Create()
    {
      Map = items => items
        .Select(x => new
        {
          Dependencies = x.Dependencies
        });
    }
  }
}