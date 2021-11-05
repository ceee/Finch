using System;
using zero.Core.Routing;

namespace zero.Core.Extensions
{
  public static class RouteExtensions
  {   
    public static Route DependsOn(this Route route, params string[] ids)
    {
      route.Dependencies.AddRange(ids);
      return route;
    }

    public static Route DependsOn(this Route route, Route anotherRoute)
    {
      route.Dependencies.AddRange(anotherRoute.Dependencies);
      
      foreach ((string key, object value) in anotherRoute.Params)
      {
        route.Params[key] = value;
      }

      return route;
    }

    public static Route Param(this Route route, string key, object value)
    {
      route.Params[key] = value;
      return route;
    }

    public static object Param(this Route route, string key)
    {
      return route.Params.TryGetValue(key, out object val) ? val : default;
    }

    public static T Param<T>(this Route route, string key)
    {
      return route.Params.GetValueOrDefault<T>(key);
    }

    [Obsolete]
    public static Route Reference(this Route route, string id, string collection)
    {
      route.References.Add(new(id, collection));
      return route;
    }
  }
}
