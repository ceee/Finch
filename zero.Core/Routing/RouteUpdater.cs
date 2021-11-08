using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class RouteUpdater<TProvider, TEntity> : RouteUpdater<TProvider, TEntity, TEntity>
    where TProvider : ZeroRouteProvider 
    where TEntity : IZeroRouteEntity
  {
    public RouteUpdater(TProvider provider) : base(provider) { }
  }


  public class RouteUpdater<TProvider, TAffectedEntity, TTargetEntity> : RouteUpdater
    where TProvider : ZeroRouteProvider
    where TAffectedEntity : IZeroRouteEntity
    where TTargetEntity : IZeroRouteEntity
  {
    public TProvider Provider { get; protected set; }

    public RouteUpdater(TProvider provider)
    {
      Provider = provider;
    }


    public override bool CanHandle<T>(T model)
    {
      return typeof(TAffectedEntity).GetTypeInfo().IsAssignableFrom(typeof(T));
    }


    public virtual Task<RouteUpdaterResult> OnDelete(RouteUpdaterContext context, TAffectedEntity model) => base.OnDelete(context, model);

    public override sealed Task<RouteUpdaterResult> OnDelete(RouteUpdaterContext context, IZeroRouteEntity model) => OnDelete(context, (TAffectedEntity)model);

    public virtual async Task<RouteUpdaterResult> OnUpdate(RouteUpdaterContext context, TAffectedEntity model, TAffectedEntity previousModel = default)
    {
      RouteUpdaterResult result = await OnDelete(context, model);

      List<Route> updates = new();
      IEnumerable<TTargetEntity> affectedItems = (await context.Session.LoadAsync<TTargetEntity>(context.AffectedRoutes.Select(x => x.ReferenceId).ToArray())).Select(x => x.Value);

      foreach (TTargetEntity affectedItem in affectedItems)
      {
        Route route = context.AffectedRoutes.FirstOrDefault(x => x.ReferenceId == affectedItem.Id);
        Route newRoute = await Provider.Create(context, affectedItem);

        if (newRoute != null)
        {
          result.Updated.Add(route.Update(newRoute));
        }
      }

      return result;
    }

    public override sealed Task<RouteUpdaterResult> OnUpdate(RouteUpdaterContext context, IZeroRouteEntity model, IZeroRouteEntity previousModel = null) => OnUpdate(context, (TAffectedEntity)model, (TAffectedEntity)previousModel);
  }


  public abstract class RouteUpdater
  {
    public virtual bool CanHandle<T>(T model) where T : IZeroRouteEntity => false;


    public virtual Task<RouteUpdaterResult> OnDelete(RouteUpdaterContext context, IZeroRouteEntity model)
    {
      RouteUpdaterResult result = new();
      result.Removed.AddRange(context.AffectedRoutes);
      return Task.FromResult(result);
    }

    public abstract Task<RouteUpdaterResult> OnUpdate(RouteUpdaterContext context, IZeroRouteEntity model, IZeroRouteEntity previousModel = default);
  }


  public class RouteUpdaterContext : RoutingContext
  {
    public RouteUpdaterContext(IZeroStore store, IZeroContext context, IZeroDocumentSession session) : base(store, context, session) { }

    public IEnumerable<Route> AffectedRoutes { get; set; }
  }


  public class RouteUpdaterResult
  {
    public List<Route> Removed { get; private set; } = new();

    public List<Route> Updated { get; private set; } = new();
  }
}
