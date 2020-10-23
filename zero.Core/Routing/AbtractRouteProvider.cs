using Raven.Client.Documents.Session;
using System;
using System.Threading.Tasks;

namespace zero.Core.Routing
{
  public abstract class AbtractRouteProvider<T> : IRouteProvider<T>, IRouteProvider
  {
    public virtual string Alias { get; }

    public virtual Type[] AffectedTypes { get; }


    public AbtractRouteProvider(string alias)
    {
      Alias = alias;
      AffectedTypes = new Type[1] { typeof(T) }; 
    }


    /// <inheritdoc />
    public abstract Task<IRoute> GetRoute(IAsyncDocumentSession session, T model);


    /// <inheritdoc />
    public async Task<IRoute> GetRoute(IAsyncDocumentSession session, object model)
    {
      if (!(model is T))
      {
        return null;
      }
      return await GetRoute(session, (T)model);
    }


    /// <inheritdoc />
    public virtual Task<IResolvedRoute> ResolveRoute(IAsyncDocumentSession session, IRoute route)
    {
      DefaultResolvedRoute resolved = new DefaultResolvedRoute() { Route = route };
      return Task.FromResult((IResolvedRoute)resolved);
    }


    /// <inheritdoc />
    public string GetRouteId(T model)
    {
      throw new NotImplementedException();
    }


    /// <inheritdoc />
    public string GetRouteId(object model)
    {
      throw new NotImplementedException();
    }
  }
}
