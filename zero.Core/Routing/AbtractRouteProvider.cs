using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace zero.Core.Routing
{
  public abstract class AbtractRouteProvider<T> : IRouteProvider<T>
  {
    public virtual string Alias { get; protected set; }

    public virtual Type[] AffectedTypes { get; protected set; }

    public virtual string Controller { get; protected set; }

    public virtual string Action { get; protected set; }

    public const string ID_PREFIX = "routes.";

    public const char SLASH = '/';


    public AbtractRouteProvider(string alias)
    {
      Alias = alias;
      AffectedTypes = new Type[1] { typeof(T) }; 
      Controller = "DefaultRoute";
      Action = "Index";
    }


    /// <inheritdoc />
    public virtual async Task<IRoute> GetRoute(IAsyncDocumentSession session, T model)
    {
      return await session.LoadAsync<IRoute>(GetRouteId(model));
    }


    /// <inheritdoc />
    public virtual async Task<IRoute> GetRoute(IAsyncDocumentSession session, object model)
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
    public abstract Task<IList<IRoute>> GetAllRoutes(IAsyncDocumentSession session);


    /// <inheritdoc />
    public abstract string GetRouteId(T model);


    /// <inheritdoc />
    public string GetRouteId(object model)
    {
      if (!(model is T))
      {
        throw new ArgumentException($"Parameter has to be of type {typeof(T)}", nameof(model));
      }

      return GetRouteId((T)model);
    }
  }
}
