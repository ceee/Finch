using zero.Core.Database;
using zero.Core.Entities;

namespace zero.Core.Caches.Internal
{
  public class ApplicationCache : InternalEntityCache<Application>
  {
    public ApplicationCache(IZeroStore store) : base(store) { }
  }
}
