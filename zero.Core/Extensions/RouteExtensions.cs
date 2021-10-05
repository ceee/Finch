using zero.Core.Routing;

namespace zero.Core.Extensions
{
  public static class RouteExtensions
  {   
    public static Route DependsOn(this Route route, string id)
    {
      route.Dependencies.Add(id);
      return route;
    }

    public static Route Param(this Route route, string key, object value)
    {
      route.Params[key] = value;
      return route;
    }

    public static Route Reference(this Route route, string id, string collection)
    {
      route.References.Add(new(id, collection));
      return route;
    }
  }
}
