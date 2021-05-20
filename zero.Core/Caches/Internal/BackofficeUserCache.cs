using System.Collections.Generic;
using zero.Core.Database;
using zero.Core.Entities;

namespace zero.Core.Caches.Internal
{
  public class BackofficeUserCache : InternalEntityCache<BackofficeUserCache.Result, BackofficeUser>
  {
    public BackofficeUserCache(IZeroStore store) : base(store) { }

    public class Result : ZeroIdEntity
    {
      public string AppId { get; set; }
      public string CurrentAppId { get; set; }
      public bool IsSuper { get; set; }
      public List<UserClaim> Claims { get; set; } = new();
    }
  }
}
