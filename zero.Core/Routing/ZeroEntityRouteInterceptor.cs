using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class ZeroEntityRouteInterceptor : CollectionInterceptor
  {
    protected ILogger<ZeroEntityRouteInterceptor> Logger { get; set; }


    public ZeroEntityRouteInterceptor(ILogger<ZeroEntityRouteInterceptor> logger)
    {
      Logger = logger;
    }


    public override Task Saved(SaveParameters args)
    {
      Logger.LogDebug("intercept: " + args.Model.Name + " (" + args.Model.Id + ") - " + args.Model.GetType());
      return base.Saved(args);
    }
  }
}
