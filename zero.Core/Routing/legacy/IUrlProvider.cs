using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace zero.Core.Routing
{
  public interface IUrlProvider<T> where T : IResolvedRoute
  {
    string Alias { get; }

    Task<T> Resolve(IRoute route);

    Task<IList<IRoute>> Run();
  }
}
