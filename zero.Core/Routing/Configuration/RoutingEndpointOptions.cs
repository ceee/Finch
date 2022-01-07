namespace zero.Routing;

public class RoutingEndpointOptions
{
  HashSet<ResolverMap> Resolvers { get; set; } = new();

  class ResolverMap
  {
    public Type Type { get; set; }
    public Func<IRouteModel, IRouteEndpoint> Impl { get; set; }
  }


  public void Add<T>(Func<T, IRouteEndpoint> resolver) where T : class, IRouteModel
  {
    Resolvers.Add(new()
    {
      Type = typeof(T),
      Impl = obj => resolver(obj as T)
    });
  }

  public void Replace<T>(Func<T, IRouteEndpoint> resolver) where T : class, IRouteModel
  {
    Remove<T>();
    Add(resolver);
  }

  public void Remove<T>() where T : IRouteModel
  {
    Type type = typeof(T);
    Resolvers.RemoveWhere(x => x.Type == type);
  }

  public Func<IRouteModel, IRouteEndpoint> Get<T>() where T : IRouteModel => Get(typeof(T));

  public IEnumerable<Func<IRouteModel, IRouteEndpoint>> GetAll<T>() where T : IRouteModel => GetAll(typeof(T));

  public Func<IRouteModel, IRouteEndpoint> Get(Type type)
  {
    ResolverMap map = Resolvers.LastOrDefault(x => x.Type == type);
    return map?.Impl;
  }

  public IEnumerable<Func<IRouteModel, IRouteEndpoint>> GetAll(Type type)
  {
    IEnumerable<ResolverMap> maps = Resolvers.Where(x => x.Type == type);
    return maps.Select(map => map.Impl).Where(x => x != null);
  }
}
