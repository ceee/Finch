using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using zero.Core.Entities;

namespace zero.Core.Options
{
  public class RoutingPageResolverOptions
  {
    HashSet<ResolverMap> Resolvers { get; set; } = new();

    class ResolverMap
    {
      public Type Type { get; set; }
      public Expression<Func<IPage, bool>> Impl { get; set; }
    }


    public void Add<T>(Expression<Func<IPage, bool>> resolver)
    {
      Resolvers.Add(new()
      {
        Type = typeof(T),
        Impl = resolver
      });
    }

    public void Replace<T>(Expression<Func<IPage, bool>> resolver)
    {
      Remove<T>();
      Add<T>(resolver);
    }

    public void Remove<T>()
    {
      Type type = typeof(T);
      Resolvers.RemoveWhere(x => x.Type == type);
    }

    public Expression<Func<IPage, bool>> Get<T>() => Get(typeof(T));

    public IEnumerable<Expression<Func<IPage, bool>>> GetAll<T>() => GetAll(typeof(T));

    public Expression<Func<IPage, bool>> Get(Type type)
    {
      ResolverMap map = Resolvers.LastOrDefault(x => x.Type == type);
      return map?.Impl;
    }

    public IEnumerable<Expression<Func<IPage, bool>>> GetAll(Type type)
    {
      IEnumerable<ResolverMap> maps = Resolvers.Where(x => x.Type == type);
      return maps.Select(map => map.Impl).Where(x => x != null);
    }
  }
}
