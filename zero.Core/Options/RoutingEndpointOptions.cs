using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Routing;

namespace zero.Core.Options
{
  public class RoutingEndpointOptions
  {
    HashSet<ResolverMap> Resolvers { get; set; } = new();

    class ResolverMap
    {
      public Type Type { get; set; }
      public Func<IResolvedRoute, RouteProviderEndpoint> Impl { get; set; }
    }


    public void Add<T>(Func<T, RouteProviderEndpoint> resolver) where T : class, IResolvedRoute
    {
      Resolvers.Add(new()
      {
        Type = typeof(T),
        Impl = obj => resolver(obj as T)
      });
    }

    public void Replace<T>(Func<T, RouteProviderEndpoint> resolver) where T : class, IResolvedRoute
    {
      Remove<T>();
      Add(resolver);
    }

    public void Remove<T>() where T : IResolvedRoute
    {
      Type type = typeof(T);
      Resolvers.RemoveWhere(x => x.Type == type);
    }

    public Func<IResolvedRoute, RouteProviderEndpoint> Get<T>() where T : IResolvedRoute => Get(typeof(T));

    public IEnumerable<Func<IResolvedRoute, RouteProviderEndpoint>> GetAll<T>() where T : IResolvedRoute => GetAll(typeof(T));

    public Func<IResolvedRoute, RouteProviderEndpoint> Get(Type type)
    {
      ResolverMap map = Resolvers.LastOrDefault(x => x.Type == type);
      return map?.Impl;
    }

    public IEnumerable<Func<IResolvedRoute, RouteProviderEndpoint>> GetAll(Type type)
    {
      IEnumerable<ResolverMap> maps = Resolvers.Where(x => x.Type == type);
      return maps.Select(map => map.Impl).Where(x => x != null);
    }
  }
}
