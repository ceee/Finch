using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public abstract class ZeroRouteProvider<T> : ZeroRouteProvider, IRouteProvider<T> where T : IZeroRouteEntity
  {
    public ZeroRouteProvider(string alias) : base(alias) { }

    /// <inheritdoc />
    public override bool CanHandle(Type type) => typeof(T).IsAssignableFrom(type);

    /// <inheritdoc />
    public abstract Task<Route> Create(RoutingContext context, T model);

    /// <inheritdoc />
    public sealed override Task<Route> Create(RoutingContext context, IZeroRouteEntity model) => Create(context, (T)model);

    /// <inheritdoc />
    public virtual string Key(T model) => model.Hash;

    /// <inheritdoc />
    public sealed override string Key(IZeroRouteEntity model) => Key((T)model);

    /// <inheritdoc />
    public virtual Task<bool> IsRouteStale(RoutingContext context, T previous, T current) => Task.FromResult(true);

    /// <inheritdoc />
    public sealed override Task<bool> IsRouteStale(RoutingContext context, IZeroRouteEntity previous, IZeroRouteEntity current) => IsRouteStale(context, (T)previous, (T)current);

    /// <inheritdoc />
    public virtual string Id(T model) => base.Id(model);

    /// <inheritdoc />
    public sealed override string Id(IZeroRouteEntity model) => base.Id(model);

    /// <inheritdoc />
    public virtual async Task<Route> Find(RoutingContext context, T model) => await context.Session.LoadAsync<Route>(Id(model));

    /// <inheritdoc />
    public sealed override Task<Route> Find(RoutingContext context, IZeroRouteEntity model) => Find(context, (T)model);
  }


  public abstract class ZeroRouteProvider : IRouteProvider
  {
    protected static string ID_PARAM = "id";

    protected PageRouteResolverHelper PageResolver { get; set; } = new();

    public ZeroRouteProvider(string alias)
    {
      Alias = alias;
    }

    public virtual string Alias { get; protected set; }

    /// <inheritdoc />
    public virtual bool CanHandle(Type type) => false;

    /// <inheritdoc />
    public abstract Task<Route> Create(RoutingContext context, IZeroRouteEntity model);

    /// <inheritdoc />
    public virtual string Key(IZeroRouteEntity model) => model.Hash;

    /// <inheritdoc />
    public virtual IAsyncEnumerable<Route> Seed(RoutingContext context) => AsyncEnumerable.Empty<Route>();

    /// <inheritdoc />
    public virtual Task<IRouteModel> Model(RoutingContext context, Route route) => Task.FromResult((IRouteModel)new RouteModel() { Route = route });

    /// <inheritdoc />
    public virtual Task<bool> IsRouteStale(RoutingContext context, IZeroRouteEntity previous, IZeroRouteEntity current) => Task.FromResult(true);

    /// <inheritdoc />
    public virtual string Id(IZeroRouteEntity model) => "routes." + Alias + "." + Key(model);

    /// <inheritdoc />
    public virtual RouteEndpoint Map(RoutingContext context, IRouteModel route)
    {
      IEnumerable<Func<IRouteModel, RouteEndpoint>> resolvers = context.Context.Options.Routing.EndpointResolvers.GetAll(route.GetType());

      foreach (Func<IRouteModel, RouteEndpoint> resolver in resolvers.Reverse())
      {
        RouteEndpoint endpoint = resolver(route);

        if (endpoint != null)
        {
          return endpoint;
        }
      }

      return context.Context.Options.Routing.DefaultEndpoint;
    }

    /// <summary>
    /// Find a persisted route for an entity
    /// </summary>
    public virtual async Task<Route> Find(RoutingContext context, IZeroRouteEntity model) => await context.Session.LoadAsync<Route>(Id(model));
  }


  public interface IRouteProvider<T> : IRouteProvider where T : IZeroRouteEntity
  {
    /// <summary>
    /// Generate unique route key for a model
    /// </summary>
    string Key(T model);

    /// <summary>
    /// Generate unique ID for a model
    /// </summary>
    string Id(T model);

    /// <summary>
    /// Create route entity from a model
    /// </summary>
    Task<Route> Create(RoutingContext context, T model);

    /// <summary>
    /// Determines whether the route for previous is stale and needs to be refreshed 
    /// based on comparison with the previous version
    /// </summary>
    Task<bool> IsRouteStale(RoutingContext context, T previous, T current);

    /// <summary>
    /// Find a persisted route for an entity
    /// </summary>
    Task<Route> Find(RoutingContext context, T model);
  }


  public interface IRouteProvider
  {
    /// <summary>
    /// Alias of this route provider
    /// </summary>
    string Alias { get; }

    /// <summary>
    /// Whether this provider can handle a certain entity type
    /// </summary>
    bool CanHandle(Type type);

    /// <summary>
    /// Generate unique route key for a model
    /// </summary>
    string Key(IZeroRouteEntity model);

    /// <summary>
    /// Generate unique ID for a model
    /// </summary>
    string Id(IZeroRouteEntity model);

    /// <summary>
    /// Create route entity from a model
    /// </summary>
    Task<Route> Create(RoutingContext context, IZeroRouteEntity model);

    /// <summary>
    /// Get all models which should be provided and handled by this instance
    /// </summary>
    IAsyncEnumerable<Route> Seed(RoutingContext context);

    /// <summary>
    /// Converts a route to a model which is passed to the endpoint
    /// </summary>
    Task<IRouteModel> Model(RoutingContext context, Route route);

    /// <summary>
    /// Determines whether the route for previous is stale and needs to be refreshed 
    /// based on comparison with the previous version
    /// </summary>
    Task<bool> IsRouteStale(RoutingContext context, IZeroRouteEntity previous, IZeroRouteEntity current);

    /// <summary>
    /// Map a route model to an endpoint
    /// </summary>
    RouteEndpoint Map(RoutingContext context, IRouteModel route);

    /// <summary>
    /// Find a persisted route for an entity
    /// </summary>
    Task<Route> Find(RoutingContext context, IZeroRouteEntity model);
  }

}
