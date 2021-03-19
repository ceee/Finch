using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Collections;

namespace zero.Core.Subscriptions
{
  public class MediaBlueprintSubscription : CollectionInterceptor
  {
    public override Task Created<T>(CreateParameters<T> args)
    {
      return base.Created(args);
    }
  }
}
