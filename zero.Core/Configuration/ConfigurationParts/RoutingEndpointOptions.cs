using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Routing;

namespace zero;

public class RoutingEndpointOptions
{
  HashSet<ResolverMap> Resolvers { get; set; } = new();

  class ResolverMap
  {
    public Type Type { get; set; }
    public Func<IRouteModel, RouteEndpoint> Impl { get; set; }
  }


  public void Add<T>(Func<T, RouteEndpoint> resolver) where T : class, IRouteModel
  {
    Resolvers.Add(new()
    {
      Type = typeof(T),
      Impl = obj => resolver(obj as T)
    });
  }

  public void Replace<T>(Func<T, RouteEndpoint> resolver) where T : class, IRouteModel
  {
    Remove<T>();
    Add(resolver);
  }

  public void Remove<T>() where T : IRouteModel
  {
    Type type = typeof(T);
    Resolvers.RemoveWhere(x => x.Type == type);
  }

  public Func<IRouteModel, RouteEndpoint> Get<T>() where T : IRouteModel => Get(typeof(T));

  public IEnumerable<Func<IRouteModel, RouteEndpoint>> GetAll<T>() where T : IRouteModel => GetAll(typeof(T));

  public Func<IRouteModel, RouteEndpoint> Get(Type type)
  {
    ResolverMap map = Resolvers.LastOrDefault(x => x.Type == type);
    return map?.Impl;
  }

  public IEnumerable<Func<IRouteModel, RouteEndpoint>> GetAll(Type type)
  {
    IEnumerable<ResolverMap> maps = Resolvers.Where(x => x.Type == type);
    return maps.Select(map => map.Impl).Where(x => x != null);
  }
}
